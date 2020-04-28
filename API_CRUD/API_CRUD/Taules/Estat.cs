using NPoco;
using System;
using System.Collections.Generic;

namespace Taules
{

    [NPoco.TableName("Estat")]
    [NPoco.PrimaryKey("ID", AutoIncrement = true)]
    [ExplicitColumns]
    public class Estat
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("NOM")]
        public string Nom { get; set; }


        public override string ToString()
        {
            return
                "ID: " + ID + " | " +
                "NOM: " + Nom + " |";
        }

        public static implicit operator Estat(List<Estat> v)
        {
            throw new NotImplementedException();
        }
    }

}



