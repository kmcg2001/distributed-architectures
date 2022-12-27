using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Class1
    {
        uint blockID; // unique
        uint fromWalletID; 
        uint toWalletID;
        float amount; // cannot be negative
        uint blockOffset; // used to produce hash,  has to be multiple of 5
        byte[] prevBlockHash; // A hash that starts with 12345
        byte[] currentBlockHash; // A hash that starts with 12345
    }
}
