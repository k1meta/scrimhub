import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';

type HealthResponse = {
  status: string;
  service: string;
};

@Injectable({ providedIn: 'root' })
export class Health {
  private readonly http = inject(HttpClient);
  readonly status = signal('loading');

  load(): void {
    this.http.get<HealthResponse>('/api/health').subscribe({
      next: (body) => this.status.set(body.status),
      error: () => this.status.set('unavailable'),
    });
  }
}
