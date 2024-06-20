using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Sudoku
{
    public class SettingViewPresenter : AggregateRoot
    {
        public SettingViewPresenter(SettingView view, DomainEventService service) : base(service)
        {
            View  = view;
            
            service.Register<InitScene> (Init   , Declarations.Sudoku);
            service.Register<StartScene>(Start  , Declarations.Sudoku);
            service.Register<Setting>   (Setting, Declarations.Sudoku);
        }

        public SettingView View  { get; }
        
        private int _Size;
        private int _Display;
        private int _Tips;

        private int MinDisplay => (int)Mathf.Pow(_Size, 4) / 10;
        private int MaxDisPlay => (int)(Mathf.Pow(_Size, 4) * 0.4f);

        private SudokuSetup Setup => new SudokuSetup(_Size, _Display, _Tips);

        public void Init(InitScene init) 
        {
            var adapters = View.GetComponentsInChildren<IListenerAdapter>();

            var dropdowns = adapters
                .OfType<DropdownListener>()
                .ToDictionary(k => k.Id);
            var sizes = Declarations.Sizes;
            
            var size    = dropdowns[0];
            var display = dropdowns[1];
            
            size.AddOption(sizes.Select(s => string.Format("{0} x {0}", s)));
            size.AddListener((value) =>
            {
                _Size    = sizes[(int)value];
                _Tips    = _Size;
                _Display = MinDisplay;
                display.Listener.SetValueWithoutNotify(0);
                display.AddOption(CreateList(MinDisplay, MaxDisPlay).ConvertAll(s => s.ToString()));
            });

            _Size    = sizes[0];
            _Display = MinDisplay;
            _Tips    = _Size;

            display.AddOption(CreateList(MinDisplay, MaxDisPlay).ConvertAll(s => s.ToString()));
            display.AddListener((value) => _Display = MinDisplay + (int)value);

            adapters
                .OfType<ButtonListener>()
                .First()
                .AddListener((id) =>
                {
                    this.SettleEvents(Declarations.Sudoku, Setup);
                    
                    View.Close();
                });
        }
        
        public void Setting(Setting setting) 
        {
            View.Open();
        }

        public void Start(StartScene start) 
        {
            View.Open();
        }

        private List<int> CreateList(int start, int end) 
        {
            var list = new List<int>();

            for(int num = start; num <= end; num++) 
            {
                list.Add(num);  
            }

            return list;
        }
    }

    public class Setting : DomainEventBase 
    {
        
    }
}