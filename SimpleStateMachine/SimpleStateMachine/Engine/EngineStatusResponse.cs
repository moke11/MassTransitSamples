using System;

namespace SimpleStateMachine.Engine
{
    public class EngineStatusResponse
    {
        public string State { get; set; }
        public Guid EngineId { get; set; }
    }
}