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

        private void CellViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Handle the PropertyChanged event of the CellViewModel
            // Update the UI based on the property change, if needed
            if(e.PropertyName == nameof(CellSection.Value))
            {
                sudokuBoard.ValidateBoard();
            }
        }

        private void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            BoardSection sudokuBoard = this.sudokuBoard as BoardSection;

            switch (sudokuBoard.boardState.GetStateName())
            {
                case "HelperState":
                    sudokuBoard.boardState = new NormalState();
                    break;
                case "NormalState":
                    sudokuBoard.boardState = new HelperState();
                    break;
                default:
                    sudokuBoard.boardState = new NormalState();
                    break;
            }
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
                    sudokuBoard.SetBoardContent(content);

                    // Update the UI to reflect the new board
                    GenerateNormalBoardUI();

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
