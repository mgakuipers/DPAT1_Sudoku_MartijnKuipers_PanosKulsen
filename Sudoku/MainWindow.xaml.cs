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
            sudokuBoard = gameController.CreateNineByNineBoard();

            // Add event handler for board state change
            // sudokuBoard.BoardStateChanged += SudokuBoard_BoardStateChanged;

            // Populate the UI with the Sudoku board cells
            GenerateNormalBoardUI();
        }

        private void GenerateNormalBoardUI()
        {
            // Clear the existing UI elements
            gridBoard.Children.Clear();

            // Iterate over the cells in the Sudoku board and add UI elements for each cell
            for (int row = 0; row < sudokuBoard.GetSize(); row++)
            {
                for (int col = 0; col < sudokuBoard.GetSize(); col++)
                {
                    // Get the cell value from the Sudoku board
                    CellSection cell = sudokuBoard.GetCell(row, col);
                    int cellValue = cell.Value;
                    double cellSize = 35;

                    // Create an instance of the CellView
                    CellView cellView = new CellView();
                    cellView.cell.Width = cellSize;
                    cellView.cell.Height = cellSize;

                    // Set the DataContext of the CellView to the corresponding CellViewModel
                    CellViewModel cellViewModel = new CellViewModel(cell);
                    cellViewModel.Value = cellValue; 
                    cellViewModel.PropertyChanged += CellViewModel_PropertyChanged; // Subscribe to the PropertyChanged event
                    cellView.DataContext = cellViewModel;

                    // Position the TextBox on the Canvas
                    double left = col * cellSize;
                    double top = row * cellSize;
                    Canvas.SetLeft(cellView, left);
                    Canvas.SetTop(cellView, top);

                    // Add the TextBox to the Canvas
                    gridBoard.Children.Add(cellView);
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

        private void SudokuBoard_BoardStateChanged(object sender, EventArgs e)
        {
            // Handle the Sudoku board state change event
            // Update the UI based on the new state
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
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
                        sudokuBoard = SudokuGameController.Instance.CreateFourByFourBoard();
                        break;
                    case ".6x6":
                        sudokuBoard = SudokuGameController.Instance.CreateSixBySixBoard();
                        break;
                    case ".9x9":
                        sudokuBoard = SudokuGameController.Instance.CreateNineByNineBoard();
                        break;
                    case ".jigsaw":
                        // Handle jigsaw board format
                        break;
                    case ".samurai":
                        // Handle samurai board format
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
