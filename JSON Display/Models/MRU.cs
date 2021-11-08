using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Display.Models
{
    // Handles both files and 
public class MRU
{
    public string Name { get; set; }
    public string Address { get; set; }

    public string Data { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(Name);

    public bool IsFile => !string.IsNullOrWhiteSpace(Address);

    public bool IsData => !IsFile;

    public MRU(string address)
    {
        if (string.IsNullOrWhiteSpace(address)
            || !File.Exists(address))  return;
            
        Address = address;

        Name = System.IO.Path.GetFileName(address);
    }

    public override string ToString() => Name;
}
}
