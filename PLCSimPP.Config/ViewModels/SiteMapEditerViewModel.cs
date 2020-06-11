using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Helper;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Config.ViewDatas;
using BCI.PLCSimPP.Service.Devices;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using GC = BCI.PLCSimPP.Service.Devices.GC;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class SiteMapEditerViewModel : BindableBase
    {
        private readonly IEventAggregator mEventAggregator;
        private readonly IConfigService mConfigService;

        #region properites


        private bool mIsInEdit;
        public bool IsInEdit
        {
            get { return mIsInEdit; }
            set
            {
                SetProperty(ref mIsInEdit, value);
                mSaveBtnText = mIsInEdit ? "Save" : "Add";
                RaisePropertyChanged("SaveBtnText");
            }
        }

        private string mSaveBtnText = "Add";
        public string SaveBtnText
        {
            get { return mSaveBtnText; }
            set { SetProperty(ref mSaveBtnText, value); }
        }

        private int mPort;

        public int Port
        {
            get { return mPort; }
            set { SetProperty(ref mPort, value); }
        }

        private UnitType mType;

        public UnitType Type
        {
            get { return mType; }
            set { SetProperty(ref mType, value); }
        }

        private string mAddress = "";

        public string Address
        {
            get { return mAddress; }
            set { SetProperty(ref mAddress, value); }
        }

        private string mName = "";

        public string Name
        {
            get { return mName; }
            set { SetProperty(ref mName, value); }
        }

        private bool mIsMaster;

        public bool IsMaster
        {
            get { return mIsMaster; }
            set { SetProperty(ref mIsMaster, value); }
        }

        private IUnit mSelectedUnit;

        public IUnit SelectedUnit
        {
            get { return mSelectedUnit; }
            set { SetProperty(ref mSelectedUnit, value); }
        }

        public ObservableCollection<IUnit> Port1 { get; set; }
        public ObservableCollection<IUnit> Port2 { get; set; }
        public ObservableCollection<IUnit> Port3 { get; set; }
        public List<UnitTypeInfo> UnitTypeList { get; set; }
        public List<int> PortList { get; set; }

        #endregion

        public ICommand RemoveCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand SelectUnitCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }

        public SiteMapEditerViewModel(IEventAggregator eventAggr, IConfigService configService)
        {
            mEventAggregator = eventAggr;
            mConfigService = configService;

            Port1 = new ObservableCollection<IUnit>();
            Port2 = new ObservableCollection<IUnit>();
            Port3 = new ObservableCollection<IUnit>();
            InitUnitType();

            RemoveCommand = new DelegateCommand<IUnit>(DoRemove);
            MoveUpCommand = new DelegateCommand<IUnit>(DoMoveUp);
            MoveDownCommand = new DelegateCommand<IUnit>(DoMoveDown);
            AddCommand = new DelegateCommand(DoAdd);
            ClearAllCommand = new DelegateCommand(DoClearAll);
            SaveCommand = new DelegateCommand(DoSave);
            CancelCommand = new DelegateCommand(DoCancel);
            CancelEditCommand = new DelegateCommand(DoCancelEdit);
            SelectUnitCommand = new DelegateCommand(DoSelectUnit);

            mEventAggregator.GetEvent<LoadDataEvent>().Subscribe(OnLoad);
        }

        private void OnLoad(string param)
        {
            var units = mConfigService.ReadSiteMap();


            foreach (var item in units)
            {
                if (item.Port == 1)
                {
                    SetPortCollection(item, Port1);
                }
                else if (item.Port == 2)
                {
                    SetPortCollection(item, Port2);
                }
                else if (item.Port == 3)
                {
                    SetPortCollection(item, Port3);
                }
            }
        }

        private void SetPortCollection(IUnit item, ObservableCollection<IUnit> portCollection)
        {
            portCollection.Clear();
            portCollection.Add(item);

            if (item.HasChild)
            {
                item.IsMaster = true;
                foreach (var subItem in item.Children)
                {
                    portCollection.Add(subItem);
                }
            }
        }

        private void DoMoveDown(IUnit unit)
        {
            if (unit.Port == 1)
            {
                MoveDown(Port1, unit);
            }
            if (unit.Port == 2)
            {
                MoveDown(Port2, unit);
            }
            if (unit.Port == 3)
            {
                MoveDown(Port3, unit);
            }
        }

        private void MoveUp(ObservableCollection<IUnit> port, IUnit unit)
        {
            int index = port.IndexOf(unit);
            if (index > 0)
            {
                port.RemoveAt(index);
                port.Insert(index - 1, unit);
            }
        }

        private void DoMoveUp(IUnit unit)
        {
            if (Port == 1)
            {
                MoveUp(Port1, unit);
            }
            if (Port == 2)
            {
                MoveUp(Port2, unit);
            }
            if (Port == 3)
            {
                MoveUp(Port3, unit);
            }
        }

        private void MoveDown(ObservableCollection<IUnit> port, IUnit unit)
        {
            int index = port.IndexOf(unit);
            if (index < port.Count)
            {
                port.RemoveAt(index);
                if (index + 1 > port.Count)
                    port.Insert(index, unit);
                else
                    port.Insert(index + 1, unit);
            }
        }

        private void DoSelectUnit()
        {
            if (SelectedUnit == null)
            {
                return;
            }

            IsInEdit = true;

            Port = SelectedUnit.Port;
            Type = GetUnitType(SelectedUnit.GetType());
            Address = SelectedUnit.Address;
            Name = SelectedUnit.DisplayName;
        }

        private UnitType GetUnitType(Type type)
        {
            if (type == typeof(Aliquoter))
                return UnitType.Aliquoter;
            if (type == typeof(Centrifuge))
                return UnitType.Centrifuge;
            if (type == typeof(DxC))
                return UnitType.DxC;
            if (type == typeof(DynamicInlet))
                return UnitType.DynamicInlet;
            if (type == typeof(GC))
                return UnitType.GC;
            if (type == typeof(HLane))
                return UnitType.HLane;
            if (type == typeof(HMOutlet))
                return UnitType.HMOutlet;
            if (type == typeof(ILane))
                return UnitType.ILane;
            if (type == typeof(Labeler))
                return UnitType.Labeler;
            if (type == typeof(LevelDetector))
                return UnitType.LevelDetector;
            if (type == typeof(Outlet))
                return UnitType.Outlet;
            if (type == typeof(ErrorLane))
                return UnitType.ErrorLane;
            return UnitType.Stocker;
        }

        private void DoCancelEdit()
        {
            IsInEdit = false;
            SelectedUnit = null;
        }

        private void DoCancel()
        {
            mEventAggregator.GetEvent<NavigateEvent>().Publish(ViewName.CONFIGURATION);
        }

        private void DoSave()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select the save path";
            sfd.Filter = "XML file (*.xml)|*.xml";
            sfd.CheckPathExists = true;
            sfd.DefaultExt = "xml";
            sfd.RestoreDirectory = true;
            sfd.ShowDialog();

            if (string.IsNullOrEmpty(sfd.FileName))
            {
                return;
            }

            List<IUnit> units = new List<IUnit>();
            if (Port1.Count > 0)
            {
                var master1 = Port1.First();
                master1.Children.Clear();
                for (int i = 1; i < Port1.Count; i++)
                {
                    master1.Children.Add(Port1[i]);
                }
                units.Add(master1);
            }

            if (Port2.Count > 0)
            {
                var master2 = Port2.First();
                master2.Children.Clear();
                for (int i = 1; i < Port2.Count; i++)
                {
                    master2.Children.Add(Port2[i]);
                }
                units.Add(master2);
            }

            if (Port3.Count > 0)
            {
                var master3 = Port3.First();
                master3.Children.Clear();
                for (int i = 1; i < Port3.Count; i++)
                {
                    master3.Children.Add(Port3[i]);
                }
                units.Add(master3);
            }
            
            mConfigService.SaveSiteMap(sfd.FileName, units);
        }

        private void DoClearAll()
        {
            //double check
            var result = MessageBox.Show("Confirm to remove All?", "Confirm", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            Port1.Clear();
            Port2.Clear();
            Port3.Clear();
        }

        private void InitUnitType()
        {
            PortList = new List<int> { 1, 2, 3 };
            Port = 1;

            UnitTypeList = new List<UnitTypeInfo>();
            var array = Enum.GetValues(typeof(UnitType));
            foreach (UnitType value in array)
            {
                UnitTypeList.Add(new UnitTypeInfo { Name = EnumHelper.GetEnumDescription(value), Value = value });
            }
        }

        /// <summary>
        /// CheckPortDuplication
        /// </summary>
        /// <param name="address"></param>
        /// <returns>return true if is duplicate</returns>
        private bool CheckDuplication(string address)
        {
            return CheckPortDuplication(address, Port1)
                || CheckPortDuplication(address, Port2)
                || CheckPortDuplication(address, Port3);
        }

        /// <summary>
        /// CheckPortDuplication
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns>return true if is duplicate</returns>
        private bool CheckPortDuplication(string address, ObservableCollection<IUnit> port)
        {
            return port.Any(u => u.Address == address);
        }

        private void DoAdd()
        {
            if (Address.Length < 10)
            {
                Address = Address.PadLeft(10, '0');
            }

            if (CheckDuplication(Address))
            {
                MessageBox.Show("The unit address cannot be repeated");
                return;
            }

            if (IsInEdit)
            {
                SelectedUnit.Address = Address;
                SelectedUnit.DisplayName = Name;
                IsInEdit = false;
                SelectedUnit = null;
                return;
            }

            IUnit unit = GetUnitByType(Type);
            unit.Address = Address;
            unit.DisplayName = Name;
            unit.Port = Port;
            unit.IsMaster = IsMaster;

            if (Port == 1)
            {
                AddToPort(Port1, unit);
            }
            if (Port == 2)
            {
                AddToPort(Port2, unit);
            }
            if (Port == 3)
            {
                AddToPort(Port3, unit);
            }
        }

        private void AddToPort(ObservableCollection<IUnit> port, IUnit unit)
        {
            var haveMaster = port.Any(u => u.IsMaster);
            if (IsMaster)
            {
                if (haveMaster)
                {
                    MessageBox.Show("There can only be one master unit per Port");
                    return;
                }
                port.Add(unit);
            }
            else
            {
                if (!haveMaster)
                {
                    MessageBox.Show("Each Port must first have a master unit");
                    return;
                }
                port.Add(unit);
            }
        }

        private IUnit GetUnitByType(UnitType type)
        {
            switch (type)
            {
                case UnitType.Aliquoter:
                    return new Aliquoter();
                case UnitType.Centrifuge:
                    return new Centrifuge();
                case UnitType.DxC:
                    return new DxC();
                case UnitType.DynamicInlet:
                    return new DynamicInlet();
                case UnitType.GC:
                    return new GC();
                case UnitType.HLane:
                    return new HLane();
                case UnitType.HMOutlet:
                    return new HMOutlet();
                case UnitType.ILane:
                    return new ILane();
                case UnitType.Labeler:
                    return new Labeler();
                case UnitType.LevelDetector:
                    return new LevelDetector();
                case UnitType.Outlet:
                    return new Outlet();
                case UnitType.Stocker:
                    return new Stocker();
                case UnitType.ErrorLane:
                    return new ErrorLane();
                default:
                    return new GC();
            }
        }

        /// <summary>
        /// remove unit
        /// </summary>
        /// <param name="unit">unit</param>
        private void DoRemove(IUnit unit)
        {
            if (unit.Port == 1)
            {
                RemoveFromPort(Port1, unit);
            }
            if (unit.Port == 2)
            {
                RemoveFromPort(Port2, unit);
            }
            if (unit.Port == 3)
            {
                RemoveFromPort(Port3, unit);
            }
        }

        /// <summary>
        /// remove unit from port
        /// </summary>
        /// <param name="port">port number</param>
        /// <param name="unit">unit</param>
        private void RemoveFromPort(ObservableCollection<IUnit> port, IUnit unit)
        {
            if (unit.IsMaster)
            {
                //double check
                var result = MessageBox.Show($"Confirm to remove Port {unit.Port}?", "Confirm", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                port.Clear();
            }
            port.Remove(unit);
        }
    }
}
