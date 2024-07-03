using Cosmos.System;
using Cosmos.System.Graphics;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables
{
    public class Variable
    {
        public Variable() { }
        public Variable(string ID, string Value, VariableType Type)
        {
            this.ID = ID;
            this.Value = Value;
            this.Type = Type;
        }
        public Variable(string ID, int Value, VariableType Type)
        {
            this.ID = ID;
            this.IntValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, bool Value, VariableType Type)
        {
            this.ID = ID;
            this.BoolValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, float Value, VariableType Type)
        {
            this.ID = ID;
            this.FloatValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, double Value, VariableType Type)
        {
            this.ID = ID;
            this.DoubleValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, char Value, VariableType Type)
        {
            this.ID = ID;
            this.CharValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, Bitmap Value, VariableType Type)
        {
            this.ID = ID;
            this.BitmapValue = Value;
            this.Type = Type;
        }
        public Variable(string ID, ConsoleKeyEx Value, VariableType Type)
        {
            this.ID = ID;
            this.ConsoleKeyEx = Value;
            this.Type = Type;
        }
        public Variable(string ID, List<Variable> Vars, VariableType Type)
        {
            this.ID = ID;
            this.Vars = Vars;
            this.Type = Type;
        }
        //Name of the variable
        public string ID { get; set; }
        //Value of the variable if it is a string
        public string Value { get; set; }
        //Value of the variable if it is an integer
        public int IntValue { get; set; }
        //Value of the variable if it is a boolean
        public bool BoolValue { get; set; }
        //Value of the variable if it is a float
        public float FloatValue { get; set; }
        //Value of the variable if it is a double
        public double DoubleValue { get; set; }
        //Value of the variable if it is a char
        public char CharValue { get; set; }
        public Bitmap BitmapValue { get; set; }
        //Keypress event
        public ConsoleKeyEx ConsoleKeyEx { get; set; }
        //List implementation
        public List<Variable> Vars { get; set; }
        //TypeOf the variable
        public VariableType Type { get; set; }
    }
    public enum VariableType
    {
        String,
        Int,
        Bool,
        Float,
        Double,
        Char,
        ConsoleKeyEx,
        Bitmap,
        List
    }
}
