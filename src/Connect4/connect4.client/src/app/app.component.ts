import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Cell {
  state: 'empty' | 'red' | 'yellow';
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  public board: Cell[][] = Array.from({ length: 7 }, (_, i) =>
    Array.from({ length: 6 }, (_, i) => ({ state: 'empty' }))
  );

  constructor(private http: HttpClient) {}

  private currentPlayer: 'red' | 'yellow' = 'red';

  public cellClicked(rowIndex: number, cellIndex: number) {
    this.board[rowIndex][cellIndex].state = this.currentPlayer;
    this.currentPlayer = this.currentPlayer === 'red' ? 'yellow' : 'red';
  }

  ngOnInit() {}
}
