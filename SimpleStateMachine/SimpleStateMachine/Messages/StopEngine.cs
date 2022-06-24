using System;

namespace SimpleStateMachine.Messages
{
    public class StopEngine
    {
        public Guid EngineId { get; set; }
    }
}