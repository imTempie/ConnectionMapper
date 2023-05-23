using System;
using PacketDotNet;
using SharpPcap;
using IP2Location;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Cryptography.X509Certificates;
using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.WindowsForms;
using GMap.NET;
using Microsoft.Maui.Controls.Maps;
using System.Xml.Linq;
using Microsoft.Maui.Maps;
using GMap.NET.WindowsForms.Markers;
using System.Collections.Generic;
using System.Collections;

namespace NetworkMapperForms

{
    public static class State
    {
        public static Dictionary<string, Details> InboundConnections = new Dictionary<string, Details>();
        public static Dictionary<string, Details> OutboundConnections = new Dictionary<string, Details>();
    }

    public class PCap
    {
        // Used to stop the capture loop
        public static String GetExtIp()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string externalIpString = a3[0];
            return externalIpString;
        }

        private bool _stopCapturing;
        public void capturePackets(ILiveDevice device, StringOutputType selectedOutputType, Func<string, string> output) { 
            
            // Console.CancelKeyPress += HandleCancelKeyPress;

            // Open the device for packet capturing
            var readTimeoutMilliseconds = 1000;
            device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

            output("");
            output($"-- Listening on {device.Description}...");

            output("Loading IP database...");
            
            var ipv4Geo = new IP2Location.Component();
            ipv4Geo.Open(@".\db\IP2LOCATION-LITE-DB5.BIN\IP2LOCATION-LITE-DB5.BIN");

            var ipv6Geo = new IP2Location.Component();
            ipv6Geo.Open(@".\db\IP2LOCATION-LITE-DB5.IPV6.BIN\IP2LOCATION-LITE-DB5.IPV6.BIN");
            
            output("IP database loaded!");

            // Get external ip
            var externalIpString = GetExtIp();

            // Get local lat/long with external ip
            var localGeoResult = ipv4Geo.IPQuery(externalIpString.ToString());
            // IP2Location.IPResult localGeoResult = new IPResult();
            
            var sourceLat = new float();
            var sourceLong = new float();
            var destinationLat = new float();
            var destinationLong = new float();

            while (_stopCapturing == false)
            {
                PacketCapture e;
                var status = device.GetNextPacket(out e);

                // Null packets can be returned in the case where
                // the GetNextRawPacket() timed out, we should just attempt
                // to retrieve another packet by looping the while() again
                if (status != GetPacketStatus.PacketRead)
                {
                    // Go back to the start of the while()
                    continue;
                }

                var rawCapture = e.GetPacket();

                // Use PacketDotNet to parse this packet and print out
                // its high level information
                var p = Packet.ParsePacket(rawCapture.GetLinkLayers(), rawCapture.Data);

                // Create IPResult objects for source and destination
                IP2Location.IPResult sourceResult = new IPResult();
                IP2Location.IPResult destinationResult = new IPResult();



                // If the packet is an IPv4
                if (p.PayloadPacket is IPv4Packet ipPacket)
                {
                    // Check if destination address is a local ip
                    var destinationLocal = (ipPacket.DestinationAddress.ToString().StartsWith("192.")
                        || ipPacket.DestinationAddress.ToString().StartsWith("10.")
                        || ipPacket.DestinationAddress.ToString().StartsWith("172."));

                    // Check if source address is a local a ip
                    var sourceLocal = (ipPacket.SourceAddress.ToString().StartsWith("192.")
                        || ipPacket.SourceAddress.ToString().StartsWith("10.")
                        || ipPacket.SourceAddress.ToString().StartsWith("172."));

                    // If both address' arge local ips, then ignore the packet and dont print it out (continue)
                    if (sourceLocal && destinationLocal)
                    {
                        continue;
                    }

                    // Geolocate the sourceResult and destinationResult from ip to Lat/Lon
                    sourceResult = ipv4Geo.IPQuery(ipPacket.SourceAddress.ToString());
                    destinationResult = ipv4Geo.IPQuery(ipPacket.DestinationAddress.ToString());

                    sourceLat = sourceResult.Latitude;
                    sourceLong = sourceResult.Longitude;
                    
                    destinationLat = destinationResult.Latitude;
                    destinationLong = destinationResult.Longitude;

                    // Adding inbound and outbound connections to dictionary
                    var sourceIpAddress = ipPacket.SourceAddress.ToString();
                    var destinationIpAddress = ipPacket.DestinationAddress.ToString();
                    if(sourceIpAddress == externalIpString || sourceLocal)
                    {
                        // If the dictionary doesnt contain the key
                        if (!State.OutboundConnections.ContainsKey(destinationIpAddress))
                        {
                            // Add the key and new details object to the dictionary
                            State.OutboundConnections.Add(destinationIpAddress, new Details
                            {
                                Lat = destinationLat.ToString(),
                                Long = destinationLong.ToString(),
                                LastSeen = DateTime.Now
                            });
                        } else if (State.OutboundConnections.ContainsKey(destinationIpAddress))
                        {
                            // If the ip is already in the dictionary, update last seen time
                            State.OutboundConnections[destinationIpAddress].LastSeen = DateTime.Now;
                        }
                    } else if (destinationIpAddress == externalIpString || destinationLocal)
                    {
                        // If the dictionary doesnt contain the key 
                        if (!State.InboundConnections.ContainsKey(sourceIpAddress))
                        {
                            // Add the key and new details object to the dictionary
                            State.InboundConnections.Add(sourceIpAddress, new Details
                            {
                                Lat = sourceLat.ToString(),
                                Long = sourceLong.ToString(),
                                LastSeen = DateTime.Now
                            });

                        } else if (State.InboundConnections.ContainsKey(sourceIpAddress))
                        {
                            // If the ip is already in the dictionary, update last seen time
                            State.InboundConnections[sourceIpAddress].LastSeen = DateTime.Now;
                        }
                    } else
                    {
                        continue;
                    }
                }

                // If the packet is an IPv6
                if (p.PayloadPacket is IPv6Packet ip6Packet)
                {
                    // Check if destination address is a local ip
                    var destinationLocal = (ip6Packet.DestinationAddress.ToString().StartsWith("192.")
                        || ip6Packet.DestinationAddress.ToString().StartsWith("10.")
                        || ip6Packet.DestinationAddress.ToString().StartsWith("172."));

                    // Check if source address is a local a ip
                    var sourceLocal = (ip6Packet.SourceAddress.ToString().StartsWith("192.")
                        || ip6Packet.SourceAddress.ToString().StartsWith("10.")
                        || ip6Packet.SourceAddress.ToString().StartsWith("172."));

                    // If both address' are local ips, then ignore the packet and dont print it out (continue)
                    if (ip6Packet.SourceAddress.IsIPv6LinkLocal && ip6Packet.DestinationAddress.IsIPv6LinkLocal)
                    {
                        continue;
                    }

                    // Geolocate the sourceResult and destinationResult from ip to Lat/Lon
                    sourceResult = ipv6Geo.IPQuery(ip6Packet.SourceAddress.ToString());
                    destinationResult = ipv6Geo.IPQuery(ip6Packet.DestinationAddress.ToString());

                    sourceLat = sourceResult.Latitude;
                    sourceLong = sourceResult.Longitude;

                    destinationLat = destinationResult.Latitude;
                    destinationLong = destinationResult.Longitude;

                    // Adding inbound and outbound connections to dictionary
                    var sourceIpAddress = ip6Packet.SourceAddress.ToString();
                    var destinationIpAddress = ip6Packet.DestinationAddress.ToString();
                    if (sourceIpAddress == externalIpString || sourceLocal)
                    {
                        // If the dictionary doesnt contain the key
                        if (!State.OutboundConnections.ContainsKey(destinationIpAddress))
                        {
                            // Add the key and new details object to the dictionary
                            State.OutboundConnections.Add(destinationIpAddress, new Details
                            {
                                Lat = destinationLat.ToString(),
                                Long = destinationLong.ToString(),
                                LastSeen = DateTime.Now
                            });

                        } else if(State.OutboundConnections.ContainsKey(destinationIpAddress))
                        {
                            // If the ip is already in the dictionary, update last seen time
                            State.OutboundConnections[destinationIpAddress].LastSeen = DateTime.Now;
                        }
                    }
                    else if (destinationIpAddress == externalIpString || destinationLocal)
                    {
                        // If the dictionary doesnt contain the key to the dictionary
                        if (!State.InboundConnections.ContainsKey(sourceIpAddress))
                        {
                            // Add the key and new details object
                            State.InboundConnections.Add(sourceIpAddress, new Details
                            {
                                Lat = sourceLat.ToString(),
                                Long = sourceLong.ToString(),
                                LastSeen = DateTime.Now
                            });

                        } else if(State.InboundConnections.ContainsKey(sourceIpAddress))
                        {
                            // If the ip is already in the dictionary, update last seen time
                            State.InboundConnections[sourceIpAddress].LastSeen = DateTime.Now;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                // If the lat and long is 0, this means the local ip address was fed into the geolocation db.
                // So we change it to the localGeoResult, checking both source and destination coordinates.
                if (sourceResult.Latitude == 0 && sourceResult.Longitude == 0)
                {
                    sourceResult=localGeoResult;
                }
                if (destinationResult.Latitude == 0 && destinationResult.Longitude == 0)
                {
                    destinationResult=localGeoResult;
                }
                // Write the packet info to the console
                if (sourceResult != destinationResult)
                {
                    output(p.ToString(selectedOutputType));
                } else
                {
                    continue;
                }
                // Write the lat/long of source to destination to the console
                output($"Coordinates - Source: {sourceResult.Latitude} {sourceResult.Longitude}    Destination: {destinationResult.Latitude} {destinationResult.Longitude}");
            }

            output("-- Capture stopped");

            // Close the pcap device
            device.Close();
        }
        
        public void stopCapturing()
        {
            _stopCapturing = true;
        }
    }
}
