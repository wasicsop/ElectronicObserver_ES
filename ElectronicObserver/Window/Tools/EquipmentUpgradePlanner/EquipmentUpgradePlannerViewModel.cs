﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Control.Paging;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlannerViewModel : WindowViewModelBase
{
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; }

	public PagingControlViewModel PlannedUpgradesPager { get; }

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; }

	private EquipmentPickerService EquipmentPicker { get; }

	public EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public EquipmentUpgradePlanCostViewModel TotalCost { get; private set; } = new(new());

	public GridLength PlanListWidth { get; set; } = new(350, GridUnitType.Pixel);

	public EquipmentUpgradeFilterViewModel Filters { get; set; } = new();

	public EquipmentUpgradePlannerViewModel()
	{
		PlannedUpgradesPager = new();
		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradePlanner = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		PlannedUpgrades = EquipmentUpgradePlanManager.PlannedUpgrades;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => UpdateTotalCost();
		EquipmentUpgradePlanManager.PlanCostUpdated += (_, _) => UpdateTotalCost();
		EquipmentUpgradePlanManager.PlanEquipmentMasterUpdated += (_, _) => Update();
		Filters.PropertyChanged += (_, _) => Update();
		Update();
		UpdateTotalCost();
	}

	[RelayCommand]
	private void AddEquipmentPlan()
	{
		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentId = equipment.MasterID;

			if (newPlan.OpenPlanDialog())
			{
				EquipmentUpgradePlanManager.AddPlan(newPlan);
				EquipmentUpgradePlanManager.Save();
			}
		}
	}

	[RelayCommand]
	private void AddEquipmentPlanFromMasterData()
	{
		IEquipmentDataMaster? equipment = EquipmentPicker.OpenMasterEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentMasterDataId = equipment.EquipmentId;

			if (newPlan.OpenPlanDialog())
			{
				EquipmentUpgradePlanManager.AddPlan(newPlan);
				EquipmentUpgradePlanManager.Save();
			}
		}
	}

	[RelayCommand]
	private void RemovePlan(EquipmentUpgradePlanItemViewModel planToRemove)
	{
		EquipmentUpgradePlanManager.RemovePlan(planToRemove);
		EquipmentUpgradePlanManager.Save();
	}

	[RelayCommand]
	private void OpenEditDialog(EquipmentUpgradePlanItemViewModel plan)
	{
		if (plan.OpenPlanDialog())
		{
			EquipmentUpgradePlanManager.Save();
		}
	}

	[RelayCommand]
	private void OpenTreeDialog(EquipmentUpgradePlanItemViewModel plan)
	{
		UpgradeTreeView view = new(new(plan));
		view.ShowDialog();
	}

	private void UpdateTotalCost()
	{
		TotalCost.UnsubscribeFromApis();
		TotalCost = new(PlannedUpgrades
			.Where(plan => !plan.Finished)
			.Aggregate(new EquipmentUpgradePlanCostModel(), (cost, viewModel) => cost + viewModel.Cost.Model, totalCost => totalCost));
	}

	private void Update()
	{
		PlannedUpgradesPager.Items = PlannedUpgrades
			.Where(Filters.MeetsFilterCondition)
			.OrderBy(plan => plan.Finished)
			.Cast<object>()
			.ToList();
	}
}
