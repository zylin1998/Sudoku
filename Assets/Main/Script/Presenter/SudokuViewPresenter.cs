using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Loyufei;
using Loyufei.DomainEvents;
using UnityEngine.UI;

namespace Sudoku
{
    public class SudokuViewPresenter : AggregateRoot
    {
        public SudokuViewPresenter(SudokuView view, DomainEventService service) : base(service)
        {
            View  = view;

            service.Register<InitScene>  (Init        , Sudoku);
            service.Register<SudokuSetup>(CreateMetrix, Sudoku);
        }

        private int                  _Size;
        private int                  _Tips;
        private bool                 _Fulfilled;
        private float                _StartTime;
        private List<NumberListener> _Numbers;
        private List<ReviewListener> _Reviews;
        private OptionListener       _ConstructListener;
        private OptionListener       _TipsListener;
        private OptionListener       _DisplayListener;
        private OptionListener       _QuitListener;

        public SudokuView View   { get; }
        public object     Sudoku => Declarations.Sudoku;

        public void Init(InitScene init) 
        {
            var setting = new Setting();
            var quit    = new SendMessage("離開遊戲", Application.Quit);

            var options = View
                .GetComponentsInChildren<OptionListener>()
                .ToDictionary(k => k.Id);

            _ConstructListener = options[0];
            _TipsListener      = options[1];
            _DisplayListener   = options[2];
            _QuitListener      = options[3];

            _ConstructListener
                .AddListener((param) => SettleEvents(setting));
            _TipsListener
                .AddListener((param) => Tips());
            _DisplayListener
                .AddListener((param) => DisplayAll());
            _QuitListener
                .AddListener((param) => SettleEvents(quit));

            _TipsListener   .Listener.interactable = false;
            _DisplayListener.Listener.interactable = false;
        }

        public void Display(NumberListener number) 
        {
            var query = new Query(number.Id, (num) => number.Display(num));

            SettleEvents(query);
        }

        public void DisplayAll()
        {
            var queryAll = new QueryAll((answers) 
                =>
                {
                    var dic = answers.ToDictionary(k => k.Identity, v => v.Data);

                    _Numbers.ForEach(number => number.Display(dic.GetorReturn(number.Id, () => 0))); 
                });

            _TipsListener   .Listener.interactable = false;
            _DisplayListener.Listener.interactable = false;

            SettleEvents(queryAll);
        }

        public void Check(NumberListener number) 
        {
            Warning(number);

            Verify();
        }

        private void Warning(NumberListener main) 
        {
            if (main.Context <= 0) { return; }

            var length = (int)Mathf.Pow(_Size, 2);

            var stacks = _Numbers.Where(check =>
            {
                if (Equals(main, check)) { return false; }

                var q1 = main.Id  / length;
                var r1 = main.Id  % length;
                var q2 = check.Id / length;
                var r2 = check.Id % length;
                
                var row    = Equals(q1, q2);
                var column = Equals(r1, r2);
                var area   = Equals(main.AreaId, check.AreaId);
                
                return row || column || area;
            }).ToList();

            if (stacks.Any())
            {
                var warning = false;

                stacks.ForEach(s =>
                {
                    if (Equals(s.Context, main.Context))
                    {
                        warning = true;

                        s.Warning(false);
                    }
                });

                if (warning) { main.Warning(true); }
            }
        }

        private void Verify() 
        {
            if (_Numbers.Any(number => number.Context == 0)) { return; }

            var list     = new List<int>();
            var queryAll = new QueryAll((answers) =>
            {
                list.AddRange(answers.Select(answer => answer.Data));
            });

            SettleEvents(queryAll);

            _Fulfilled = true;

            foreach (var number in _Numbers)
            {
                _Fulfilled = Equals(number.Context, list[number.Id]);

                if (!_Fulfilled) { return; }
            }

            var passTime = Time.realtimeSinceStartup - _StartTime;

            string message 
                = string.Format("恭喜完成\n使用時間：{0}：{1}", (int)passTime / 60, (int)passTime % 60);

            if (_Fulfilled) { SettleEvents(new SendMessage(message)); }
        }

        public void Tips() 
        {
            var empty = _Numbers
                .Where(number => number.Context == 0)
                .ToList();

            var random = Declarations.GetRandom(0, empty.Count - 1);
            var number = empty[random];

            Display(number);

            _Tips--;

            _TipsListener.Listener.interactable = _Tips > 0;
            _TipsListener.SetText(string.Format("提示 x {0}", _Tips));
        }

        public void CreateMetrix(SudokuSetup setup)
        {
            Remove();

            Construct(setup.Size, setup.Tips, setup.Display);
        }

        private void Remove() 
        {
            View.Remove();

            _Numbers?.Clear();
            _Reviews?.Clear();
        }

        private void Construct(int size, int tips, int display) 
        {
            SettleEvents(new ConstructMetrix(size));

            _Size = size;
            _Tips = tips;

            View.Layout(size);

            _Numbers = View.Numbers.ToList();
            _Reviews = View.Reviews.ToList();

            _TipsListener.SetText(string.Format("提示 x {0}", _Tips));

            var random = Declarations.RandomList(0, (int)Mathf.Pow(size, 4), display);

            random.Sort((n1, n2) => n1.CompareTo(n2));

            var toggleGroup = View.GetComponentInChildren<ToggleGroup>();

            _Reviews.ForEach(review =>
            {
                review.AddListener((isOn) =>
                {
                    _Numbers
                        .Where(number => Equals(number.Context, review.Id))
                        .ForEach(number => number.Review((bool)isOn));
                });

                review.Listener.group = toggleGroup;
            });

            _Numbers.ForEach(number =>
            {
                number.AddListener((id) => SettleEvents(new InputMask((num) =>
                {
                    number.Interact(num);

                    Check(number);
                })));

                if (random.Contains(number.Id)) { Display(number); }

                else { number.Clear(); }
            });

            _DisplayListener.Listener.interactable = true;
            _TipsListener   .Listener.interactable = true;
            
            var index = 0;
            var duration = 2f / _Numbers.Count;
            Observable
                .Interval(TimeSpan.FromSeconds(duration))
                .TakeWhile((time) => index < _Numbers.Count)
                .Subscribe(time =>
                {
                    _Numbers[index++].gameObject.SetActive(true);
                });

            _Fulfilled = false;
        }

        private void SettleEvents(params IDomainEvent[] events) 
        {
            this.SettleEvents(Sudoku, events);
        }
    }
}