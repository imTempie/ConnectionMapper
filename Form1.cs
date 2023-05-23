using IP2Location;
using PacketDotNet;
using SharpPcap;
using System.Net;
using System;
using System.Net.Http;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using IP2Location;
using GMap.NET.WindowsForms.ToolTips;
using Microsoft.Maui.Controls.Maps;
using System.Xml.Linq;
using Microsoft.Maui.Maps;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace NetworkMapperForms
{

    public partial class Form1 : Form
    {
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
        bool capturing;
        private void capturePacketsBtn_Click(object sender, EventArgs e)
        {
            capturing = true;
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
            //var ipv6Geo = new IP2Location.Component();
            //ipv4Geo.Open(@".\db\IP2LOCATION-LITE-DB5.IPV6.BIN\IP2LOCATION-LITE-DB5.IPV6.BIN");

            // Get external ip
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string externalIpString = a3[0];

            // Get local lat/long with external ip
            var localGeoResult = ipv4Geo.IPQuery(externalIpString.ToString());

            // Add local marker
            GMapOverlay markers = new GMapOverlay("markers");
            GMapMarker marker = new GMarkerGoogle(
                new PointLatLng(localGeoResult.Latitude, localGeoResult.Longitude),
                GMarkerGoogleType.blue_pushpin);
            markers.Markers.Add(marker);
            gmap.Overlays.Add(markers);


        }

        //private void DrawMarkers()
        //{

        //    while (capturing)
        //    {
        //        foreach (var key in State.OutboundConnections.Keys)
        //        {

        //            var outbound = State.OutboundConnections[key];

        //            GMapOverlay outboundMarkers = new GMapOverlay("outboundMarkers");
        //            GMapMarker outboundMarker = new GMarkerGoogle(
        //                new PointLatLng(float.Parse(outbound.Lat), float.Parse(outbound.Long)),
        //                GMarkerGoogleType.red_pushpin);
        //            outboundMarkers.Markers.Add(outboundMarker);
        //            gmap.Overlays.Add(outboundMarkers);
        //        }
        //        foreach (var key in State.InboundConnections.Keys)
        //        {
        //            var inbound = State.InboundConnections[key];

        //            GMapOverlay inboundMarkers = new GMapOverlay("outboundMarkers");
        //            GMapMarker inboundMarker = new GMarkerGoogle(
        //                new PointLatLng(float.Parse(inbound.Lat), float.Parse(inbound.Long)),
        //                GMarkerGoogleType.red_pushpin);
        //            inboundMarkers.Markers.Add(inboundMarker);
        //            gmap.Overlays.Add(inboundMarkers);
        //        }
        //    }
        //}

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //capturing = true;
            //var devices = CaptureDeviceList.Instance;
            //var deviceIndex = captureDevice.SelectedIndex;
            //_device = devices[deviceIndex];
            //resultBox.Text += $"{_device.Description}";

            int ii = 0;
            pcap.capturePackets(_device, StringOutputType.Normal,
                (output) =>
                {
                    resultBox.Invoke((MethodInvoker)delegate
                    {
                        // This code will run on the UI thread
                        resultBox.AppendText(output + Environment.NewLine);
                        resultBox.AppendText(Environment.NewLine);
                        this.Refresh();
                    });
                    foreach (var key in State.OutboundConnections.Keys)
                    {
                        if (State.OutboundConnections[key].Marked == false)
                        {
                            var outbound = State.OutboundConnections[key];
                            GMapOverlay outboundMarkers = new GMapOverlay("outboundMarkers");
                            GMapMarker outboundMarker = new GMarkerGoogle(
                                new PointLatLng(float.Parse(outbound.Lat), float.Parse(outbound.Long)),
                                GMarkerGoogleType.red_pushpin);
                            gmap.Overlays.Add(outboundMarkers);
                            outboundMarkers.Markers.Add(outboundMarker);
                            State.OutboundConnections[key].Marked = true;
                        }
                    }
                    foreach (var key in State.InboundConnections.Keys)
                    {
                        if (State.InboundConnections[key].Marked == false)
                        {
                            var inbound = State.InboundConnections[key];
                            GMapOverlay inboundMarkers = new GMapOverlay("inboundMarkers");
                            GMapMarker inboundMarker = new GMarkerGoogle(
                                new PointLatLng(float.Parse(inbound.Lat), float.Parse(inbound.Long)),
                                GMarkerGoogleType.green_pushpin);
                            gmap.Overlays.Add(inboundMarkers);
                            inboundMarkers.Markers.Add(inboundMarker);
                            State.InboundConnections[key].Marked = true;
                        }
                    }
                    if (ii++ == 100)
                    {
                        pcap.stopCapturing();
                        capturing = false;
                    }
                    return "";
                });


        }
        //public void displayInfo()
        //{
        //    capturing = true;
        //    var devices = CaptureDeviceList.Instance;
        //    var deviceIndex = captureDevice.SelectedIndex;
        //    _device = devices[deviceIndex];
        //    resultBox.Text += $"{_device.Description}";
        //    int ii = 0;
        //    pcap.capturePackets(_device, StringOutputType.Normal,
        //        (output) =>
        //        {
        //            resultBox.AppendText(output + Environment.NewLine);
        //            resultBox.AppendText(Environment.NewLine);
        //            this.Refresh();
        //            if (ii++ == 100)
        //            {
        //                pcap.stopCapturing();
        //                capturing = false;
        //            }
        //            return "";
        //        });

        //    DrawMarkers();
        //}

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}