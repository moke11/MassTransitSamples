using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.TestFramework;
using MassTransit.Testing;
using NUnit.Framework;
using SimpleStateMachine.Car;

namespace SimpleStateMachine.Tests
{
    [TestFixture]
    public class CarStateMachineTests : InMemoryTestFixture
    {
        [Test]
        public async Task When_car_started_and_stopped()
        {
            var correlationId = NewId.NextGuid();
            var startCar = new StartCar()
            {
                CorrelationId = correlationId
            };

            await Bus.Publish(startCar);

            Guid? saga = await repository.ShouldContainSaga(correlationId, TestTimeout);
            Assert.IsTrue(saga.HasValue);

            await Bus.Publish(new BreakPressed()
            {
                CorrelationId = correlationId
            });

            saga = await repository.ShouldContainSagaInState(correlationId, machine, x => x.Final, TestTimeout);
            Assert.IsTrue(saga.HasValue);
        }

        [Test]
        public async Task When_tire_rolling()
        {
            var correlationId = NewId.NextGuid();
            var startCar = new StartCar()
            {
                CorrelationId = correlationId
            };

            await Bus.Publish(startCar);
            Guid? saga = await repository.ShouldContainSaga(correlationId, TestTimeout);
            Assert.IsTrue(saga.HasValue);

            await Bus.Publish(new PedalPressed()
            {
                CorrelationId = correlationId
            });
            saga = await repository.ShouldContainSagaInState(correlationId, machine, x => x.OnTheGas, TestTimeout);
            Assert.IsTrue(saga.HasValue);

            await Bus.Publish(new TireRolling()
            {
                CorrelationId = correlationId,
                Position = TirePosition.FrontLeft
            });
            await Bus.Publish(new TireRolling()
            {
                CorrelationId = correlationId,
                Position = TirePosition.FrontRight
            });
            await Bus.Publish(new TireRolling()
            {
                CorrelationId = correlationId,
                Position = TirePosition.RearLeft
            });
            await Bus.Publish(new TireRolling()
            {
                CorrelationId = correlationId,
                Position = TirePosition.RearRight
            });

            saga = await repository.ShouldContainSagaInState(correlationId, machine, x => x.Moving, TestTimeout);
            Assert.IsTrue(saga.HasValue);

            await Bus.Publish(new BreakPressed()
            {
                CorrelationId = correlationId
            });

            saga = await repository.ShouldContainSagaInState(correlationId, machine, x => x.Final, TestTimeout);
            Assert.IsTrue(saga.HasValue);
        }

        protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
        {
            configurator.StateMachineSaga(machine, repository);
        }

        readonly CarStateMachine machine;
        readonly InMemorySagaRepository<CarState> repository;

        public CarStateMachineTests()
        {
            machine = new CarStateMachine();
            repository = new InMemorySagaRepository<CarState>();
        }
    }
}