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
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using GC = BCI.PLCSimPP.Service.Devices.GC;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class SiteMapEditerViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator mEventAggregator;
        private readonly IConfigService mConfigService;

        #region properites

        //private string mFilePath;

        //public string FilePath
        //{
        //    get { return mFilePath; }
        //    set { SetProperty(ref mFilePath, value); }
        //}


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
        public ICommand SaveAsCommand { get; set; }
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
            SaveAsCommand = new DelegateCommand(DoSaveAs);
            CancelCommand = new DelegateCommand(DoCancel);
            CancelEditCommand = new DelegateCommand(DoCancelEdit);
            SelectUnitCommand = new DelegateCommand(DoSelectUnit);

        }



        private void OnLoad(string filePath)
        {
            Title = filePath;
            Port1.Clear();
            Port2.Clear();
            Port3.Clear();

            var units = mConfigService.ReadSiteMap(Title);

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

        #region Unit control

        private void SetPortCollection(IUnit item, ObservableCollection<IUnit> portCollection)
        {
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
            if (unit.Port == 1)
            {
                MoveUp(Port1, unit);
            }
            if (unit.Port == 2)
            {
                MoveUp(Port2, unit);
            }
            if (unit.Port == 3)
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
            IsMaster = SelectedUnit.IsMaster;
        }

        /// <summary>
        /// cancel right panel edit status
        /// </summary>
        private void DoCancelEdit()
        {
            IsInEdit = false;
            SelectedUnit = null;
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
            DoCancelEdit();
        }

        private void DoAdd()
        {
            if (Address.Length < 10)
            {
                Address = Address.PadLeft(10, '0');
            }

            if (IsInEdit)
            {
                SelectedUnit.Address = Address;
                SelectedUnit.DisplayName = Name;
                IsInEdit = false;
                SelectedUnit = null;
                return;
            }

            if (CheckDuplication(Address))
            {
                System.Windows.MessageBox.Show("The unit address cannot be repeated");
                return;
            }

            IUnit unit = GetUnitByType(Type);
            unit.Address = Address;
            unit.DisplayName = string.IsNullOrEmpty(Name) ? EnumHelper.GetEnumDescription(Type) : Name;
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
                IsInEdit = false;
                SelectedUnit = null;
            }
            else
            {
                if (SelectedUnit == unit)
                {
                    IsInEdit = false;
                    SelectedUnit = null;
                }
                port.Remove(unit);
            }
        }

        #endregion

        #region page control

        /// <summary>
        /// cancel edit site map, go back to configuration
        /// </summary>
        public void DoCancel()
        {
            if (CanCloseDialog())
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }
        }


        public void DoSave()
        {
            var units = BuildSaveList();

            mConfigService.SaveSiteMap(Title, units);

            mEventAggregator.GetEvent<ReLoadSiteMapEvent>().Publish(true);
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private void DoSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Select the save path",
                Filter = "XML file (*.xml)|*.xml",
                CheckPathExists = true,
                DefaultExt = "xml",
                RestoreDirectory = true
            };
            var result = sfd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var units = BuildSaveList();

                mConfigService.SaveSiteMap(sfd.FileName, units);

                this.Title = sfd.FileName;
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }
        }

        #endregion

        #region tool method

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

        /// <summary>
        /// build save list by port1/2/3
        /// </summary>
        /// <returns></returns>
        private List<IUnit> BuildSaveList()
        {
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

            return units;
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

        #endregion

        #region Check unit is changed

        /// <summary>
        /// Check original Setting has changed
        /// </summary>
        /// <returns>true-changed; false- no change</returns>
        public bool IsChanged()
        {
            var originalList = mConfigService.ReadSiteMap(Title).ToList();
            var targetList = BuildSaveList();

            if (originalList.Count != targetList.Count)
                return true;

            for (var i = 0; i < originalList.Count; i++)
            {
                if (!IsSame(originalList[i], targetList[i]))
                {
                    return true;
                }

                if (originalList[i].HasChild)//check children same
                {
                    if (!targetList[i].HasChild)
                        return true;

                    if (originalList[i].Children.Count != targetList[i].Children.Count)
                        return true;

                    for (var j = 0; j < originalList[i].Children.Count; j++)
                    {
                        if (!IsSame(originalList[i].Children[j], targetList[i].Children[j]))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsSame(IUnit original, IUnit target)
        {
            return original.Address == target.Address &&
                original.DisplayName == target.DisplayName &&
                original.GetType() == target.GetType() &&
                original.Port == target.Port;
        }

        #endregion

        #region IDialogAware members

        public bool CanCloseDialog()
        {
            if (!IsChanged())
            {
                return true;
            }

            var result = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                DoSave();
                return true;
            }

            if (result == MessageBoxResult.No)
            {
                OnLoad(string.Empty);
                return true;
            }

            return false;
        }

        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            //throw new NotImplementedException();
            var filePath = parameters.GetValue<string>("FilePath");
            OnLoad(filePath);
        }

        private string mTitle;
        public string Title
        {
            get { return mTitle; }
            set { SetProperty(ref mTitle, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        #endregion

    }
}
