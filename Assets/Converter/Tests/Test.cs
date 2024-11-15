using System;
using ConverterModule.Scripts;
using NUnit.Framework;
using Random = UnityEngine.Random;


namespace ConverterModule.Test
{
    public class ConverterTest
    {
        [Test]
        public void Instantiate()
        {
            var converter = new Converter(10,20,1,5, 0.2f);
            Assert.NotNull(converter);
        }

        [Test]
        [TestCase(10,20,4,9,2f)]
        [TestCase(10,200,4,90,0.03f)]
        [TestCase(2,6,1,2,1)]
        public void InstantiateSuccessful(int loadingAreaCapacity, int shippingAreaCapacity, int inputCount, int outputCount, float workTime)
        {
            var converter = new Converter(loadingAreaCapacity, shippingAreaCapacity, inputCount, outputCount, workTime);
            Assert.NotNull(converter);
        }
        [Test]
        [TestCase(1,1,1,1,0)]
        [TestCase(1,1,1,0,1)]
        [TestCase(1,1,0,1,1)]
        [TestCase(1,0,1,1,1)]
        [TestCase(0,1,1,1,1)]

        public void WhenInstantiateZeroValuesThenException(int loadingAreaCapacity, int shippingAreaCapacity, int inputCount, int outputCount, float workTime)
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var assembler = new Converter(loadingAreaCapacity, shippingAreaCapacity, inputCount, outputCount, workTime);
            });
        }
        [Test]
        [TestCase(1,1,1,1,-1)]
        [TestCase(1,1,1,-20,1)]
        [TestCase(1,1,-30,1,1)]
        [TestCase(1,-40,1,1,1)]
        [TestCase(-50,1,1,1,1)]

        public void WhenInstantiateNegativeValuesThenException(int loadingAreaCapacity, int shippingAreaCapacity, int inputCount, int outputCount, float workTime)
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var assembler = new Converter(loadingAreaCapacity, shippingAreaCapacity, inputCount, outputCount, workTime);
            });
        }

        [Test]
        public void WhenZeroLoadStartWorkTestThanZeroOutputSuccess()
        {
            var converter = new Converter(10, 20, 1, 4, 0.5f);
            Assert.NotNull(converter);

            var resultValue = 0;

            converter.OnShippingAreaChanged += i =>
            {
                resultValue = i;
            };
            converter.Start();

            var dt = 0.1f;

            for (float i = 0f; i < 10f; i += dt)
            {
                converter.Update(dt);
            }
            Assert.AreEqual(0, resultValue);
        }
        [Test]
        public void WhenInterruptCycleThanReturnToInput()
        {
            var inputCount = 1;
            var converter = new Converter(10, 20, inputCount, 4, 1f);
            Assert.NotNull(converter);
            var initialLoadCount = 5;
            converter.Load(initialLoadCount);
            converter.Start();
            converter.Update(0.5f);
            Assert.AreEqual(initialLoadCount - inputCount, converter.LoadingAreaCount);
            converter.Stop();
            Assert.AreEqual(initialLoadCount, converter.LoadingAreaCount);
        }
        [Test]
        [TestCase(1,1)]
        [TestCase(5,9)]
        [TestCase(5,6)]
        [TestCase(4,4)]
        //стопаем заводик посреди цикла и смотрим как он пытается вернуть ресурсы в переполненую зону погрузки.
        public void InputReturnBurnOutTest(int expectedOutputBurnout, int additionalLoad)
        {
            var converter = new Converter(10, 10, 5, 5, 1);
            Assert.NotNull(converter);

            var burned = 0;
            converter.BurnedInput += count =>
            {
                burned += count;
            };

            converter.Start();
            converter.Load(10);
            converter.Update(0.5f);// проходим полцикла
            converter.Load(additionalLoad); //Догружаем зону 
            converter.Update(0.1f);// еще немного поработает
            converter.Stop();//вырубаем
            Assert.AreEqual(expectedOutputBurnout,burned);
        }

        [Test]
        [TestCase(17,9)]
        [TestCase(8,6)]
        [TestCase(2,4)]
        [TestCase(2,4.5f)]
        //наш завод теперь выбрасывает мимо переполненого мусорного ведра. что с ним не так?
        public void OutputOverfillBurnOutTest(int expectedOutputBurnout, float cycles)
        {
            var cycleTime = 1f;
            var converter = new Converter(10, 10, 1, 3, cycleTime);
            Assert.NotNull(converter);

            var burned = 0;
            converter.BurnedOutput += i =>
            {
                burned += i;
            };
            converter.Load(10);
            converter.Start();
            converter.Update(cycleTime * cycles);
            Assert.AreEqual(expectedOutputBurnout,burned);
        }

        [Test]
        [TestCase(51, 270, 2, 5, 1f, 10.5f, 50, 31)]
        [TestCase(5000, 5000, 2, 1, 0.0002f, 0.04f, 200, 4600)]
        [TestCase(2, 3, 1, 1, 1f, 4f, 2, 0)]

        //это тест максимально приближенный к реальным условиям. но всё же для быстроты не используем UnityTest.
        public void EmulatedSuccessWork(int loadingAreaCapacity, int shippingAreaCapacity,
            int inputCount, int outputCount, float workCycleTime, float workTime, int expectedShipValue, int expectedLoadValue )
        {
            var converter = new Converter(loadingAreaCapacity, shippingAreaCapacity, inputCount, outputCount, workCycleTime);

            Assert.NotNull(converter);
            converter.Load(loadingAreaCapacity);

            converter.Start();

            var time = 0f;
            while (true)
            {
                var dt = 0.015f + ( 0.5f - Random.value) * 0.002f; //dt у нас будет обычная типа ~1/60 +- небольшой разброс

                //если время цикла достаточно маленькое (меньше dt) то на одну итерацию может захватить
                //больше циклов чем предполагается максимальным временем выполнения (workTime)
                //поэтому мы немного подрезаем dt так чтобы всё было ровненько
                if (time + dt > workTime)
                    dt = workTime - time;

                converter.Update(dt);
                time += dt;

                if(time >= workTime)
                    break;
            }
            converter.Stop();
            Assert.AreEqual(expectedLoadValue, converter.LoadingAreaCount);
            Assert.AreEqual(expectedShipValue, converter.ShippingAreaCount);

        }
    }

    public class ProcessorTest
    {
        [Test]
        [TestCase(1,1)]
        [TestCase(4,190)]
        [TestCase(120,1)]
        [TestCase(120000,199)]
        [TestCase(999,1000)]
        public void InstantiateSuccess(int input, int output)
        {
            var processor = new Processor(input, output, x => true, delegate {  }, delegate {  }, 1);
            Assert.NotNull(processor);
        }

        [Test]
        public void InstantiateWhenNullInputActionThanException()
        {
            Assert.Catch<NullReferenceException>(()=>
            {
                var processor = new Processor(1, 1, null, delegate { }, delegate { }, 2);
            });
        }

        [Test]
        public void InstantiateWhenNullOutputActionThanException()
        {
            Assert.Catch<NullReferenceException>(()=>
            {
                var processor = new Processor(1, 1, x => true, null, delegate { }, 2);
            });
        }

        [Test]
        public void InstantiateWhenNullStopActionThanException()
        {
            Assert.Catch<NullReferenceException>(()=>
            {
                var processor = new Processor(1, 1, x => true, delegate { }, null, 2);
            });
        }

        [Test]
        [TestCase(-3,1,1)]
        [TestCase(6,-190, 2)]
        [TestCase(1,1,-1)]
        [TestCase(0,0, 0)]
        [TestCase(1,1,-0.0001f)]
        public void InstantiateWhenZeroOrLessThanException(int input, int output, float time)
        {
            Assert.Catch<ArgumentOutOfRangeException>(()=>
            {
                var processor = new Processor(input, output, x => true, delegate { }, delegate { }, time);
            });
        }


    }

    public class CycleTimerTest
    {
        [Test]
        [TestCase(1)]
        [TestCase(0.001f)]
        [TestCase(1.621f)]

        public void InstantiateSuccess(float time)
        {
            var timer = new CycleTimer(time);
            Assert.NotNull(timer);
        }

        [Test]
        [TestCase(-0.0001f)]
        [TestCase(-1f)]
        [TestCase(0)]
        public void InstantiateWhenZeroOrLessThanException(float time)
        {
            Assert.Catch<ArgumentOutOfRangeException>(delegate { new CycleTimer(time);});
        }

        [Test]
        [TestCase(10f, 0.1f, 0.1f)]
        [TestCase(2f, 0.77f, 1f)]
        [TestCase(2f, 0.57f, 0.5f)]
        [TestCase(10f, 0.56f, 0.1f)]
        [TestCase(5f, 0.16f, 0.1f)]
        [TestCase(100f, 0.0011f, 0.1f)]
        public void RunCycleTimerCorrectTimeSuccess(float runtime, float deltaTime, float cycleTime)
        {
            var timer = new CycleTimer(cycleTime);
            Assert.NotNull(timer);

            timer.Start();

            float time = 0f;
            var dt = deltaTime;
            while (time < runtime)
            {
                if (time + dt > runtime)
                    dt = runtime - time;

                time += dt;
                timer.Update(dt);
            }
            Assert.AreEqual(runtime, timer.Runtime);
        }

        [Test]
        [TestCase(0.19f, 0, 0.2f, 0.01f)]
        [TestCase(0.19999f, 0, 0.2f, 0.01f)]
        [TestCase(0.19999999f, 0, 0.2f, 0.01f)]
        [TestCase(1.9f, 9, 0.2f, 0.1f)]
        [TestCase(2f, 5, 0.4f, 0.01f)]
        [TestCase(2f, 10, 0.2f, 0.01f)]
        [TestCase(10f, 50, 0.2f, 0.011f)]
        [TestCase(10f, 50, 0.2f, 0.013f)]
        [TestCase(10f, 50, 0.2f, 0.008f)]
        [TestCase(10f, 50, 0.2f, 0.017f)]
        [TestCase(10f, 50, 0.2f, 0.015f)]
        [TestCase(10f, 50, 0.2f, 0.003f)]

        public void RunCyclesCountConstantDeltaTimeLessThanCycle(float runtime, int cyclesExpected, float cycleTime, float deltaTime)
        {
            var actualCycles = 0;
            var timer = new CycleTimer(cycleTime);
            Assert.NotNull(timer);

            timer.OnCycle += () =>
            {
                actualCycles++;

            };

            timer.Start();

            Cycle(runtime, deltaTime, timer);

            Assert.AreEqual(runtime, timer.Runtime);
            Assert.AreEqual(cyclesExpected, actualCycles);
        }

        [Test]
        [TestCase(10f, 10, 1f)]
        [TestCase(1f, 100, 0.01f)]
        [TestCase(10f, 20, 0.5f)]
        [TestCase(10f, 50, 0.2f)]
        [TestCase(32f, 80, 0.4f)]
        [TestCase(5, 1, 4f)]
        [TestCase(3, 1000, 0.003f)]
        [TestCase(32, 3, 10f)]
        [TestCase(0.95f, 0, 1f)] 
        public void RunTimerRandomDeltaTime(float runtime, int cyclesExpected, float cycleTime)
        {
            var cycles = 0;
            var timer = new CycleTimer(cycleTime);
            Assert.NotNull(timer);

            timer.OnCycle += () =>  cycles++;
            timer.Start();

            Cycle(runtime, timer);

            Assert.AreEqual(cyclesExpected, cycles);
        }

        [Test]
        [TestCase(10f, 10, 1f)]
        [TestCase(10f, 100, 0.1f)]
        [TestCase(4f, 400, 0.01f)]

        public void SingleTimeIntervalTimerCorrectCycles(float runtime, int cyclesExpected, float cycleTime)
        {
            var cycles = 0;
            var timer = new CycleTimer(cycleTime);
            Assert.NotNull(timer);

            timer.OnCycle += () =>  cycles++;
            timer.Start();
            timer.Update(runtime);
            Assert.AreEqual(cyclesExpected, cycles);
        }

        [Test]
        [TestCase(10.5f, 6.7f, 10 + 6,  1f)]
        [TestCase(17.4f, 6.7f, 1581 + 609,  0.011f)]
        [TestCase(19, 5f, 0,  20f)]
        [TestCase(0.9999f, 1.1f, 1,  1f)]

        public void StartStopTimer(float runtime1,  float runtime2, int totalCyclesExpected, float cycleTime)
        {
            var cycles = 0;
            var timer = new CycleTimer(cycleTime);
            Assert.NotNull(timer);

            timer.OnCycle += () =>  cycles++;

            timer.Start();
            timer.Update(runtime1);
            timer.Stop();

            timer.Start();
            timer.Update(runtime2);
            timer.Stop();

            Assert.AreEqual(totalCyclesExpected, cycles);
        }
        private static void Cycle(float runtime, CycleTimer timer)
        {
            float time = 0f;

            while (time < runtime)
            {
                var dt = Random.value;

                if (time + dt > runtime)
                    dt = runtime - time;

                time += dt;
                timer.Update(dt);
            }
        }
        private static void Cycle(float runtime, float deltaTime, CycleTimer timer)
        {
            var dt = deltaTime;
            float time = 0f;

            while (time < runtime)
            {
                if (time + dt > runtime)
                    dt = runtime - time;

                timer.Update(dt);
                time += dt;
            }
        }

    }

    public class StoragesTest
    {
        [Test]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(120)]
        [TestCase(120000)]
        [TestCase(999)]
        public void InstantiateSuccess(int capacity)
        {
            var storage = new Storage(capacity);
            Assert.NotNull(storage);
        }
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-88)]
        public void InstantiateWhenZeroOrLessCapacityThenException(int capacity)
        {
            Assert.Catch<ArgumentOutOfRangeException>(()=> new Storage(capacity));
        }
        [Test]
        [TestCase(10,7,4,1)]
        [TestCase(1,9,4,4)]
        [TestCase(2,1,17,16)]

        public void PutPutTakeChangeSuccessTest(int capacity, int put1, int put2, int secondChange)
        {
            var storage = new Storage(capacity);
            storage.Put(put1, out var _);
            storage.Put(put2, out var change);
            Assert.GreaterOrEqual(change, 0);

            Assert.AreEqual(secondChange, change);
        }
        [Test]
        [TestCase(10,0)]
        [TestCase(10,-2)]
        [TestCase(10,-11)]

        public void WhenPutZeroOrLessThenException(int capacity, int put)
        {
            var storage = new Storage(capacity);
            Assert.Catch<ArgumentOutOfRangeException>(() => storage.Put(put, out var _));
        }
        
        [Test]
        [TestCase(10,10,10, true, 0)]
        [TestCase(10,1,4, false, 6)]
        [TestCase(10,11, 12, false, 10)]

        public void PutTakeLeftTest(int capacity, int put, int take, bool takeResult, int left)
        {
            var storage = new Storage(capacity);
            storage.Put(put, out var change);
            Assert.GreaterOrEqual(change, 0);
            var takeActualResult = storage.Take(take);
            Assert.AreEqual(takeResult, takeActualResult);
            
        }
    }

}
