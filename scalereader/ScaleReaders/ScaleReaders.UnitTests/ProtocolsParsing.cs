using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shkila.ScaleReaders.ScaleReaders;
using Shkila.ScaleReaders;

namespace ScaleReaders.UnitTests
{
    [TestClass]
    public class ProtocolsParsing
    {
        [TestMethod]
        public void OhausExplorerReader_ProtocolParsing_ReturnsWeightArgs()
        {
            string data = "   -59.53 g     \r\n";
            ConnectionArgs c = new ConnectionArgs();
            OhausExplorerReader reader = new OhausExplorerReader(c);
            WeightArgs wa = reader.Parse(data);
            
            WeightArgs dummy = new WeightArgs{
                Weight = "-59.53",
                Unit = "g"
            };

            Assert.AreEqual(dummy.Unit, wa.Unit);
            Assert.AreEqual(dummy.Weight, wa.Weight);
            
        }

        [TestMethod]
        public void MeravReader_ProtocolParsing_ReturnsWeightArgs()
        {
            string data = "+0000718\n";
            ConnectionArgs c = new ConnectionArgs();
            MeravReader reader = new MeravReader(c);
            WeightArgs wa = reader.Parse(data);

            WeightArgs dummy = new WeightArgs
            {
                Weight = "718",
            };

            Assert.AreEqual(dummy.Weight, wa.Weight);

        }        
    }
}
