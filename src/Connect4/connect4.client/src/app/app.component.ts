import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Cell {
  state: 'empty' | 'red' | 'yellow';
  isHovered: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private readonly rowCount = 6;

  public board: Cell[][] = Array.from({ length: this.rowCount }, (_, i) =>
    Array.from({ length: 7 }, (_, i) => ({ state: 'empty', isHovered: false }))
  );

  constructor(private http: HttpClient) {}

  public currentPlayer: 'red' | 'yellow' = 'red';

  public cellClicked(rowIndex: number, columnIndex: number) {
    let lowestFreeRow = 0;
    for(let row = 0; row < this.rowCount; row++){
      if(this.board[0][columnIndex].state !== 'empty')
        return;

      if(this.board[row][columnIndex].state === 'empty')
        lowestFreeRow = row;
      else
        break;
    }

    this.board[lowestFreeRow][columnIndex].state = this.currentPlayer;
    this.currentPlayer = this.currentPlayer === 'red' ? 'yellow' : 'red';
  }

  public mouseenter(columnIndex: number){
    for(let row = 0; row < this.rowCount; row++){
      this.board[row][columnIndex].isHovered = true;
    }
  }

  public mouseleave(columnIndex: number){
    for(let row = 0; row < this.rowCount; row++){
      this.board[row][columnIndex].isHovered = false;
    }
  }

  ngOnInit() {}
}
