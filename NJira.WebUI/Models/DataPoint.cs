﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NJira.WebUI.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(double y, string label)
        {
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double y, string legend, string label)
        {
            this.Y = y;
            this.Legend = legend;
            this.Label = label;
        }

        public DataPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public DataPoint(double x, double y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DataPoint(double x, double y, double z, string label)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Label = label;
        }

        //Explicitly setting the name to be used while serializing to JSON. 
        [DataMember(Name = "label")]
        public string Label = null;

        //Explicitly setting the name to be used while serializing to JSON. 
        [DataMember(Name = "legendText")]
        public string Legend = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "z")]
        public Nullable<double> Z = null;
    }
}