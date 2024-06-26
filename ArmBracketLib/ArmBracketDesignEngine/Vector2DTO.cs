using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class Vector2DTO
    {
        public Vector2DTO()
        {

        }

        public Vector2DTO(Vectors.Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }
        public double X { get; set; }

        public double Y { get; set; }
    }
}
