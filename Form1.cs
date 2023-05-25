using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using PacketDotNet;
using SharpPcap;

namespace NetworkMapperForms
{

    public partial class Form1 : Form
    {
        static string clickedIPAddress;
        private static PCap pcap = new PCap();
        private static ILiveDevice? _device;

        // Variables for moving the form window
        int mov;
        int movX;
        int movY;

        public Form1()
        {
            InitializeComponent();

            // Clear devices from list
            captureDevice.Items.Clear();

            var devices = CaptureDeviceList.Instance;
            var i = 0;

            // Add the devices to the device selection box list
            foreach (var dev in devices)
            {
                captureDevice.Items.Add($"{i} {dev.Description}");
                i++;
            }
        }
        private void capturePacketsBtn_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

            if (captureDevice.SelectedIndex < 0)
            {
                return;
            }

            var devices = CaptureDeviceList.Instance;
            var deviceIndex = captureDevice.SelectedIndex;
            _device = devices[deviceIndex];
            resultBox.Text += $"{_device.Description}";

            backgroundWorker1.RunWorkerAsync();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void refreshDevices_Click(object sender, EventArgs e)
        {
            // Clear the item list
            captureDevice.Items.Clear();

            var devices = CaptureDeviceList.Instance;
            var i = 0;

            // Print out the devices
            foreach (var dev in devices)
            {
                captureDevice.Items.Add($"{i} {dev.Description}");
                i++;
            }
        }

        private void resultBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // On exit button clicked, exit app
            Application.Exit();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void stopCapBtn_Click(object sender, EventArgs e)
        {
            // When the stop button is clicked, stop capturing packets
            backgroundWorker1.CancelAsync();
            pcap.stopCapturing();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gmap.SetPositionByKeywords("Ipswich, England");
            gmap.ShowCenter = false;
            gmap.Zoom = 1;
            var ipv4Geo = new IP2Location.Component();
            ipv4Geo.Open(@".\db\IP2LOCATION-LITE-DB5.BIN\IP2LOCATION-LITE-DB5.BIN");

            // Get external ip
            string externalIpString = PCap.GetExtIp();

            // Get local lat/long with external ip
            var localGeoResult = ipv4Geo.IPQuery(externalIpString.ToString());

            // Add local marker
            GMapOverlay markers = new GMapOverlay("markers");
            GMapMarker marker = new GMarkerGoogle(
                new PointLatLng(localGeoResult.Latitude, localGeoResult.Longitude),
                GMarkerGoogleType.blue_pushpin);
            markers.Markers.Add(marker);
            gmap.Overlays.Add(markers);
            marker.Tag = externalIpString + "(Your IP)";

        }

        // Create gmap overlays for markers
        GMapOverlay outboundMarkers = new GMapOverlay("outboundMarkers");
        GMapOverlay inboundMarkers = new GMapOverlay("inboundMarkers");

        public void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            pcap.capturePackets(_device, StringOutputType.Normal,
                (output) =>
                {
                    // Perform the long-running task here

                    // Update UI on the UI thread
                    resultBox.Invoke((MethodInvoker)delegate
                    {
                        // This code will run on the UI thread
                        resultBox.AppendText(output + Environment.NewLine);
                        resultBox.AppendText(Environment.NewLine);
                        this.Refresh();
                    });


                    // Add outbound markers on the UI thread
                    foreach (var key in State.OutboundConnections.Keys)
                    {
                        if (State.OutboundConnections[key].Marked == false)
                        {
                            var outbound = State.OutboundConnections[key];
                            GMapMarker outboundMarker = new GMarkerGoogle(
                                new PointLatLng(float.Parse(outbound.Lat), float.Parse(outbound.Long)),
                                GMarkerGoogleType.red_pushpin);

                            outboundMarker.Tag = key;

                            gmap.Invoke((MethodInvoker)delegate
                            {
                                // This code will run on the UI thread
                                gmap.Overlays.Add(outboundMarkers);
                                outboundMarkers.Markers.Add(outboundMarker);
                            });

                            // Attach a click event handler to the map control for marker click events
                            gmap.Invoke((MethodInvoker)delegate
                            {
                                gmap.OnMarkerClick += (marker, eventArgs) =>
                                {
                                    GMapMarker clickedMarker = marker;
                                    if (clickedMarker != null)
                                    {
                                        // Store the clicked ip address
                                        clickedIPAddress = clickedMarker.Tag.ToString();

                                        SelectedIp.Invoke((MethodInvoker)delegate
                                        {
                                            // Set the label3 text to the selected ip
                                            SelectedIp.Text = "Selected IP: " + clickedIPAddress;
                                        });
                                    }
                                };
                            });


                            State.OutboundConnections[key].Marked = true;
                        }
                    }

                    // Add inbound markers on the UI thread
                    foreach (var key in State.InboundConnections.Keys)
                    {
                        if (State.InboundConnections[key].Marked == false)
                        {
                            var inbound = State.InboundConnections[key];
                            GMapMarker inboundMarker = new GMarkerGoogle(
                                new PointLatLng(float.Parse(inbound.Lat), float.Parse(inbound.Long)),
                                GMarkerGoogleType.green_pushpin);

                            inboundMarker.Tag = key;

                            gmap.Invoke((MethodInvoker)delegate
                            {
                                // This code will run on the UI thread
                                gmap.Overlays.Add(inboundMarkers);
                                inboundMarkers.Markers.Add(inboundMarker);
                            });

                            // Attach a click event handler to the map control for marker click events
                            gmap.Invoke((MethodInvoker)delegate
                            {
                                gmap.OnMarkerClick += (marker, eventArgs) =>
                                {
                                    GMapMarker clickedMarker = marker;
                                    if (clickedMarker != null)
                                    {
                                        // Store the clicked marker's ip address
                                        clickedIPAddress = clickedMarker.Tag.ToString();

                                        SelectedIp.Invoke((MethodInvoker)delegate
                                        {
                                            // Set the SelectedIp text to the clicked marker's ip address
                                            SelectedIp.Text = "Selected IP: " + clickedIPAddress;
                                        });
                                    }
                                };
                            });

                            State.InboundConnections[key].Marked = true;
                        }
                    }
                    return "";
                });
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            // Add method to block clickedIpAddress
            // Dont allow user to block own ip address
            // Filter for externalIpString
        }

        private void UnblockButton_Click(object sender, EventArgs e)
        {
            // Add method to unblock all blocked Ip Addresses
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            outboundMarkers.Clear();
            inboundMarkers.Clear();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}