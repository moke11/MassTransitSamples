using System;

namespace SimpleStateMachine.Business
{
    public class EngineStatusResponse
    {
        public string State { get; set; }
        public Guid EngineId { get; set; }
    }
}