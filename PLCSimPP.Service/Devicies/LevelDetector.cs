using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class LevelDetector : UnitBase
    {
        public override void EnqueueSample(ISample sample)
        {
            throw new NotImplementedException();
        }

        public override void MoveSample(SortingOrder order, string bcrNo, Direction direction = Direction.Forward)
        {
            throw new NotImplementedException();
        }

        public override void OnReceivedMsg(string cmd, string content)
        {
            throw new NotImplementedException();
        }

        public override void ResetQueue()
        {
            throw new NotImplementedException();
        }

        public override bool TryDequeueSample(out ISample sample)
        {
            throw new NotImplementedException();
        }

        public LevelDetector(int port, string address, string display) : base(port, address, display)
        {

        }

        public LevelDetector() : base()
        {

        }
    }
}
