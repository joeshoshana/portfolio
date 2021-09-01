using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WeightModificationAlarm
{
    [Serializable()]
    public class Configuration
    {        
        public List<Mail> Mails;
        public int? Interval;
    }

    [Serializable()]
    public class Mail
    {
        [XmlAttribute("mail")]
        public string MailAddress;
    }
}
