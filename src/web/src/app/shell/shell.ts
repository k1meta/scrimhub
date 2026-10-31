import { Component, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Health } from '../health';

@Component({
  selector: 'app-shell',
  imports: [MatToolbarModule, MatCardModule],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell implements OnInit {
  readonly health = inject(Health);

  ngOnInit(): void {
    this.health.load();
  }
}
