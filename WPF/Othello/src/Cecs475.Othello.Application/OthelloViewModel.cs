﻿using System.Collections.Generic;
using System.Linq;
using Cecs475.Othello.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Cecs475.Othello.Application {
	public class OthelloSquare : INotifyPropertyChanged {
		private int mPlayer;
		public int Player {
			get { return mPlayer; }
			set {
				if (value != mPlayer) {
					mPlayer = value;
					OnPropertyChanged(nameof(Player));
				}
			}
		}

		public BoardPosition Position {
			get; set;
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	public class OthelloViewModel : INotifyPropertyChanged {
        private OthelloBoard mBoard;
		private ObservableCollection<OthelloSquare> mSquares;

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

        //property for returning the current player
        public int CurrentPlayer {
            get {
                return mBoard.CurrentPlayer;
            }
        }

		public OthelloViewModel() {
			mBoard = new OthelloBoard();
			mSquares = new ObservableCollection<OthelloSquare>(
				from pos in (
					from r in Enumerable.Range(0, 8)
					from c in Enumerable.Range(0, 8)
					select new BoardPosition(r, c)
				)
				select new OthelloSquare() {
					Position = pos,
					Player = mBoard.GetPieceAtPosition(pos)
				}
			);

			PossibleMoves = new HashSet<BoardPosition>(mBoard.GetPossibleMoves().Select(m => m.Position));
		}

		public void ApplyMove(BoardPosition position) {
			var possMoves = mBoard.GetPossibleMoves() as IEnumerable<OthelloMove>;
			foreach (var move in possMoves) {
				if (move.Position.Equals(position)) {
					mBoard.ApplyMove(move);
					break;
				}
			}
			PossibleMoves = new HashSet<BoardPosition>(mBoard.GetPossibleMoves().Select(m => m.Position));
            UpdateSquares();
			OnPropertyChanged(nameof(BoardValue));
            OnPropertyChanged(nameof(CurrentPlayer));
		}

        public void Undo_Last_Move() {
            if(mBoard.MoveHistory.Count > 0) {
                mBoard.UndoLastMove();
                OnPropertyChanged(nameof(BoardValue));
                PossibleMoves = new HashSet<BoardPosition>(mBoard.GetPossibleMoves().Select(m => m.Position));
                UpdateSquares();
            }
        }

        public void UpdateSquares() {
            var newSquares =
                   from r in Enumerable.Range(0, 8)
                   from c in Enumerable.Range(0, 8)
                   select new BoardPosition(r, c);
            int i = 0;
            foreach (var pos in newSquares) {
                mSquares[i].Player = mBoard.GetPieceAtPosition(pos);
                i++;
            }
        }

		public ObservableCollection<OthelloSquare> Squares {
			get { return mSquares; }
		}

		public HashSet<BoardPosition> PossibleMoves {
			get; private set;
		}

		public int BoardValue { get { return mBoard.Value; } }

	}
}
