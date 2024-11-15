using System;

namespace ConverterModule.Scripts
{
    public class Processor
    {
        private readonly int _inputCount;
        private readonly int _outputCount;
        private readonly Func<int, bool> _inputFunc;
        private readonly Action<int> _outputAction;
        private readonly float _cycleTime;

        private bool _hasPower;
        private bool _isCycleStarted;
        private float _currentTimer;
        private readonly Action<int> _stopAction;
        private float _leftOver;
        private readonly CycleTimer _timer;

        /// <summary>
        /// процессор. обладает "встроенными манипуляторами" для загрузки и выгрузки внутри сборщика
        /// </summary>
        /// <param name="inputCount">количество входных ресурсов</param>
        /// <param name="outputCount">количество выходных ресурсов</param>
        /// <param name="inputFunc">функтор, вызываемый при попытке начала цикла</param>
        /// <param name="outputAction">экшон при завершении цикла, возвращает bool</param>
        /// <param name="stopAction">экшон при досрочном завершении цикла, возвращает int</param>
        /// <param name="cycleTime">время одного цикла</param>
        public Processor(int inputCount, int outputCount, Func<int, bool> inputFunc, Action<int> outputAction,
            Action<int> stopAction, float cycleTime)
        {
            //связь через передачу методов (вместо к примеру подписки на события) избранна намеренно
            _inputFunc = inputFunc ?? throw new NullReferenceException();
            _outputAction = outputAction ?? throw new NullReferenceException();
            _stopAction = stopAction ?? throw new NullReferenceException();

            if (outputCount <= 0 || inputCount <= 0)
                throw new ArgumentOutOfRangeException();

            _inputCount = inputCount;
            _outputCount = outputCount;

            _timer = new CycleTimer(cycleTime);
            _timer.OnCycle += HandleOnCycle;
        }

        //технический метод, вызывается вне зависимости от работы
        public void Update(float dt)
        {
            if(!_hasPower)
                return;

            //если таймер не стартован, пытаемся получить входные ресурсы и если ок, то стартуем таймер
            if (!_timer.Started && _inputFunc.Invoke(_inputCount))
                _timer.Start();

            _timer.Update(dt);

        }

        //подаем питание на процессор
        public void PowerOn()
        {
            _hasPower = true;
        }

        //вырубаем электричество
        public void PowerOff()
        {
            //хотя и электричества у процессора уже нет
            //но он из последних сил делает то что что нужно

            //отдаем что забрали (если цикл начат)
            if(_timer.Started)
                _stopAction.Invoke(_inputCount);

            _timer.Stop();

            _hasPower = false;
        }

        private void HandleOnCycle()
        {
            //конец цикла

            //отдаем переработанные ресурсы. что с ними дальше будет процессору без разницы, пусть думает тот, кто передавал этот экшон
            _outputAction.Invoke(_outputCount);

            //пытаемся получить ресурсы. если не получается - выключаем таймер. процессор идет отдыхать
            if (!_inputFunc.Invoke(_inputCount))
                _timer.Stop();
        }
    }
}