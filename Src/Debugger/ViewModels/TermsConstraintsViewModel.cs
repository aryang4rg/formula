using System;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Input;

using System.Collections.ObjectModel;

using Debugger.Views;
using Debugger.ViewModels.Types;
using Debugger.Windows;
using Microsoft.Formula.API;

namespace Debugger.ViewModels;

internal class TermsConstraintsViewModel : ReactiveObject
{
    private FormulaProgram formulaProgram;
    public TermsConstraintsViewModel(MainWindow window, FormulaProgram program)
    {
        formulaProgram = program;
        
        CurrentTermItems = new ObservableCollection<Node>();
        DirectConstraintsItems = new ObservableCollection<Node>();
        PosConstraintsItems = new ObservableCollection<Node>();
        NegConstraintsItems = new ObservableCollection<Node>();
        FlatConstraintsItems = new ObservableCollection<Node>();
        CurrentTermSelectedItems = new ObservableCollection<Node>();
        
        var currentTermsBox = window.Get<TermsConstraintsView>("TermsAndConstraintsView").Get<ListBox>("CurrentTerms");
        currentTermsBox.DoubleTapped += CurrentTermsDoubleTap;
    }
    
    private void CurrentTermsDoubleTap(object? sender, TappedEventArgs e)
    {
        if (CurrentTermSelectedItems.Count > 0 &&
            CurrentTermSelectedItems[0].Id > -1)
        {
            var terms = formulaProgram.FormulaPublisher.GetLeastFixedPointConstraints();
            
            DirectConstraintsItems.Clear();
            PosConstraintsItems.Clear();
            NegConstraintsItems.Clear();
            FlatConstraintsItems.Clear();

            foreach (var constraints in terms[CurrentTermSelectedItems[0].Id][ConstraintKind.Direct])
            {
                var node = new Node(constraints);
                DirectConstraintsItems.Add(node);
            }
            
            foreach(var constraints in terms[CurrentTermSelectedItems[0].Id][ConstraintKind.Positive])
            {
                var node = new Node(constraints);
                PosConstraintsItems.Add(node);
            }
            
            foreach(var constraints in terms[CurrentTermSelectedItems[0].Id][ConstraintKind.Negative])
            {
                var node = new Node(constraints);
                NegConstraintsItems.Add(node);
            }
            
            foreach(var constraints in terms[CurrentTermSelectedItems[0].Id][ConstraintKind.Flattened])
            {
                var node = new Node(constraints);
                FlatConstraintsItems.Add(node);
            }
        }
    }

    public void ClearAll()
    {
        CurrentTermItems.Clear();
        DirectConstraintsItems.Clear();
        PosConstraintsItems.Clear();
        NegConstraintsItems.Clear();
        FlatConstraintsItems.Clear();
        CurrentTermSelectedItems.Clear();
    }
    
    public ObservableCollection<Node> CurrentTermItems { get; }
    public ObservableCollection<Node> DirectConstraintsItems { get; }
    public ObservableCollection<Node> PosConstraintsItems { get; }
    public ObservableCollection<Node> NegConstraintsItems { get; }
    public ObservableCollection<Node> FlatConstraintsItems { get; }
    public ObservableCollection<Node> CurrentTermSelectedItems { get; }
}