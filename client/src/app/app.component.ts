import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{

  ngOnInit(): void {
    this.getUsers();
  }
  title = 'DatingApp';
  http = inject(HttpClient);
  users : any;

  getUsers(){
    return this.http.get("http://localhost:5190/api/users").subscribe({
      next:(response) => {this.users=response},
      error:() => console.log(console.error),
      complete:() => {console.log('completed');
      }
      
    })
  }
}
