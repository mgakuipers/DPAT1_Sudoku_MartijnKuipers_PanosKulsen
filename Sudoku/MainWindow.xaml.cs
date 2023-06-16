using Microsoft.Win32;
using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.ViewModels;
using Sudoku.Views.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.ComponentModel;
using Sudoku.Models.Sections;
using System.Threading;
using Sudoku.Models.State;
using System.Windows.Controls.Primitives;
using System.Runtime.CompilerServices;
using Sudoku.Models.Enums;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SudokuGameController gameController;
        private IBoard sudokuBoard;

        private volatile bool isSolving = false;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the game controller
            gameController = SudokuGameController.Instance;

            // Create a new Sudoku board
            sudokuBoard = gameController.CreateNormalBoard();

            // Add event handler for board state change
            // sudokuBoard.BoardStateChanged += SudokuBoard_BoardStateChanged;

            // Populate the UI with the Sudoku board cells
            GenerateNormalBoardUI();
        }

        private void GenerateNormalBoardUI()
        {
            // Clear the existing UI elements
            gridBoard.Children.Clear();

            // Board variables
            int boardSize = sudokuBoard.GetSize();
            int verticalSize = (int)Math.Sqrt(boardSize);
            int horizontalSize = boardSize / verticalSize;

            double cellSize = 55;

            gridBoard.Height = boardSize * cellSize;
            gridBoard.Width = boardSize * cellSize;
            gridBoard.Rows = boardSize;
            gridBoard.Columns = boardSize;

            // Iterate over the cells in the Sudoku board and add UI elements for each cell
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    // Get the cell value from the Sudoku board
                    CellSection cell = sudokuBoard.GetCell(row, col);
                    int cellValue = cell.Value;

                    // Create an instance of the CellView
                    CellView cellView = new CellView();
                    cellView.cell.Width = cellSize;
                    cellView.cell.Height = cellSize;

                    cellView.PossibleNumbers.Margin = new Thickness(1, 0, 1, 0);

                    // Add border left, right, top or bottom depending on its position on the board
                    int borderThicknessNumber = 2;
                    Thickness borderThickness = new Thickness();
                    if (col % horizontalSize == 0 && col != 0)
                    {
                        borderThickness.Left = borderThicknessNumber;
                    }
                    if (col % horizontalSize == horizontalSize - 1 && col != boardSize - 1)
                    {
                        borderThickness.Right = borderThicknessNumber;
                    }
                    if (row % verticalSize == 0 && row != 0)
                    {
                        borderThickness.Top = borderThicknessNumber;
                    }
                    if (row % verticalSize == verticalSize - 1 && row != boardSize - 1)
                    {
                        borderThickness.Bottom = borderThicknessNumber;
                    }

                    // Set the DataContext of the CellView to the corresponding CellViewModel
                    CellViewModel cellViewModel = new CellViewModel(cell);
                    cellViewModel.Value = cellValue;
                    cellViewModel.PossibleNumbers = cell.PossibleNumbers;
                    cellViewModel.PropertyChanged += CellViewModel_PropertyChanged; // Subscribe to the PropertyChanged event
                    cellView.DataContext = cellViewModel;

                    // Add the TextBox to the UniformGrid
                    Border border = new Border();
                    border.BorderThickness = borderThickness;
                    border.BorderBrush = Brushes.Black;
                    border.Child = cellView;

                    gridBoard.Children.Add(border);
                }
            }
        }

        private void GenerateSamuraiBoardUI()
        {
            // Clear the existing UI elements
            gridBoard.Children.Clear();

            // Board variables
            int boardSize = sudokuBoard.GetSize();
            int verticalSize = (int)Math.Sqrt(boardSize);
            int horizontalSize = boardSize / verticalSize;

            double cellSize = 40;

            // Iterate over the cells in the Sudoku board and add UI elements for each cell
            foreach (BoardSection board in ((SamuraiBoard)sudokuBoard).boards)
            {
                UniformGrid currentBoardGrid = new UniformGrid();
                currentBoardGrid.Margin = new Thickness(0, 0, 0, 5);
                currentBoardGrid.Height = boardSize * cellSize;
                currentBoardGrid.Width = boardSize * cellSize;
                currentBoardGrid.Rows = boardSize;
                currentBoardGrid.Columns = boardSize;

                for (int row = 0; row < boardSize; row++)
                {
                    for (int col = 0; col < boardSize; col++)
                    {
                        // Get the cell value from the Sudoku board
                        CellSection cell = board.GetCell(row, col);
                        int cellValue = cell.Value;

                        // Create an instance of the CellView
                        CellView cellView = new CellView();
                        cellView.cell.Width = cellSize;
                        cellView.cell.Height = cellSize;

                        cellView.PossibleNumbers.Margin = new Thickness(1, 0, 1, 0);

                        // Add border left, right, top or bottom depending on its position on the board
                        int borderThicknessNumber = 2;
                        Thickness borderThickness = new Thickness();
                        if (col % horizontalSize == 0 && col != 0)
                        {
                            borderThickness.Left = borderThicknessNumber;
                        }
                        if (col % horizontalSize == horizontalSize - 1 && col != boardSize - 1)
                        {
                            borderThickness.Right = borderThicknessNumber;
                        }
                        if (row % verticalSize == 0 && row != 0)
                        {
                            borderThickness.Top = borderThicknessNumber;
                        }
                        if (row % verticalSize == verticalSize - 1 && row != boardSize - 1)
                        {
                            borderThickness.Bottom = borderThicknessNumber;
                        }

                        // Set the DataContext of the CellView to the corresponding CellViewModel
                        CellViewModel cellViewModel = new CellViewModel(cell);
                        cellViewModel.Value = cellValue;
                        cellViewModel.PossibleNumbers = cell.PossibleNumbers;
                        cellViewModel.PropertyChanged += CellViewModel_PropertyChanged; // Subscribe to the PropertyChanged event
                        cellView.DataContext = cellViewModel;

                        // Add the TextBox to the UniformGrid
                        Border border = new Border();
                        border.BorderThickness = borderThickness;
                        border.BorderBrush = Brushes.Black;
                        border.Child = cellView;

                        currentBoardGrid.Children.Add(border);
                    }
                }

                double left = 0.0;
                double top = 0.0;
                switch (board.SamuraiPosition)
                {
                    case SamuraiPositionEnum.TOP_LEFT:
                        left = 0.0;
                        top = 0.0;
                        break;

                    case SamuraiPositionEnum.TOP_RIGHT:
                        left = currentBoardGrid.Width + (currentBoardGrid.Width / 3);
                        top = 0.0;
                        break;

                    case SamuraiPositionEnum.CENTER:
                        left = 2 * (currentBoardGrid.Width / 3);
                        top = 2 * (currentBoardGrid.Height / 3);
                        break;

                    case SamuraiPositionEnum.BOTTOM_LEFT:
                        left = 0.0;
                        top = currentBoardGrid.Height + (currentBoardGrid.Height / 3);
                        break;

                    case SamuraiPositionEnum.BOTTOM_RIGHT:
                        left = currentBoardGrid.Width + (currentBoardGrid.Width / 3);
                        top = currentBoardGrid.Height + (currentBoardGrid.Height / 3);
                        break;
                }
                Canvas.SetLeft(currentBoardGrid, left);
                Canvas.SetTop(currentBoardGrid, top);

                samuraiGridBoard.Children.Add(currentBoardGrid);
            }
        }

        private void CellViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Handle the PropertyChanged event of the CellViewModel
            // Update the UI based on the property change, if needed
            if (e.PropertyName == nameof(CellSection.Value))
            {
                sudokuBoard.ValidateBoard();
            }
        }

        private void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            switch (sudokuBoard.GetBoardState().GetStateName())
            {
                case "HelperState":
                    sudokuBoard.SetBoardState(new NormalState());
                    btnToggleState.Content = "Switch to HelperState";
                    break;
                case "NormalState":
                    sudokuBoard.SetBoardState(new HelperState());
                    btnToggleState.Content = "Switch to NormalState";
                    break;
                default:
                    sudokuBoard.SetBoardState(new NormalState());
                    btnToggleState.Content = "Switch to HelperState";
                    break;
            }
            sudokuBoard.GetBoardState().Handle();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Sudoku Files (*.4x4;*.6x6;*.9x9;*.jigsaw;*.samurai)|*.4x4;*.6x6;*.9x9;*.jigsaw;*.samurai";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileExtension = Path.GetExtension(filePath);

                switch (fileExtension)
                {
                    case ".4x4":
                        sudokuBoard = SudokuGameController.Instance.CreateNormalBoard(4);
                        break;
                    case ".6x6":
                        sudokuBoard = SudokuGameController.Instance.CreateNormalBoard(6);
                        break;
                    case ".9x9":
                        sudokuBoard = SudokuGameController.Instance.CreateNormalBoard();
                        break;
                    case ".jigsaw":
                        sudokuBoard = SudokuGameController.Instance.CreateJigsawBoard();
                        break;
                    case ".samurai":
                        sudokuBoard = SudokuGameController.Instance.CreateSamuraiBoard();
                        break;
                    default:
                        MessageBox.Show("Unsupported file format. Please select a valid Sudoku file.");
                        return;
                }

                if (sudokuBoard != null)
                {
                    string content = File.ReadAllText(filePath);
                    content = new string(content.Where(c => !char.IsWhiteSpace(c)).ToArray<char>());
                    sudokuBoard.SetBoardContent(content);

                    // Update the UI to reflect the new board
                    switch (SudokuGameController.Instance.sudokuBoard)
                    {
                        case NormalBoard normalBoard:
                            GenerateNormalBoardUI();
                            break;

                        case SamuraiBoard samuraiBoard:
                            GenerateSamuraiBoardUI();
                            break;

                        case JigsawBoard jigsawBoard:
                            break;
                    }

                    isSolving = false;
                }
            }
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Solve" button click event
            if (!isSolving)
            {
                isSolving = true;
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    sudokuBoard.SolveBoard();
                    isSolving = false;
                }).Start();
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add option to create 4x4 and 6x6
            // Might be unnecessary check
            if (!isSolving)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    this.sudokuBoard = SudokuGameController.Instance.CreateNormalBoard();
                    Dispatcher.Invoke(() =>
                    {
                        GenerateNormalBoardUI();
                    });
                }).Start();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Reset" button click event
            if (!isSolving)
            {
                // Handle the "Reset" button click event
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    sudokuBoard.SetBoardContent(sudokuBoard.GetOriginalContent());
                    Dispatcher.Invoke(() =>
                    {
                        GenerateNormalBoardUI();
                    });
                }).Start();
            }
        }
    }
}
