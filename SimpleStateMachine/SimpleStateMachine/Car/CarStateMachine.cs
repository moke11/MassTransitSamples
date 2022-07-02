using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace SimpleStateMachine.Car
{
    public class CarStateMachine : MassTransitStateMachine<CarState>
    {
        public CarStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => Started, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => Break, x => x.CorrelateById(m => m.Message.CorrelationId));

            Initially(
                When(Started)
                    .Then(context => { Console.WriteLine($"started and running {context.Saga.CorrelationId}"); })
                    .TransitionTo(Running));

            During(Running,
                When(PedalPressed)
                    .TransitionTo(OnTheGas)
                    .Then(context =>
                    {
                        var tireEvents = context.Saga.Tires.Select(t => new RollTire()
                        {
                            Position = t.Position,
                            CorrelationId = context.Message.CorrelationId
                        });

                        foreach (var tireEvent in tireEvents)
                        {
                            context.Publish(tireEvent);
                        }
                    }));

            During(OnTheGas,
                When(TireRolling)
                    .Then(context =>
                    {
                        context.Saga.Tires.First(t => t.Position == context.Message.Position).Moving = true;

                        if (context.Saga.Tires.All(t => t.Moving))
                        {
                            context.TransitionToState(Moving);
                        }
                    }));
            
            DuringAny(
                When(Break)
                    .Then(context => { Console.WriteLine($"stopped {context.Saga.CorrelationId}"); })
                    .Finalize());

            DuringAny(
                When(StatusRequested)
                    .RespondAsync(x => x.Init<CarStatusResponse>(
                        new CarStatusResponse()
                        {
                            CorrelationId = x.Saga.CorrelationId
                        })));
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public State Running { get; }
        public State OnTheGas { get; }
        public State Moving { get; }
        
        public Event<StartCar> Started { get; }
        public Event<BreakPressed> Break { get; }
        public Event<CarStatusRequest> StatusRequested { get; }
        public Event<PedalPressed> PedalPressed { get; }

        public Event<TireRolling> TireRolling { get; }
        
    }
}