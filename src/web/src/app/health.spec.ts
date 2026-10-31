import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Health } from './health';

describe('Health', () => {
  it('records ok when the API health call succeeds', () => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });

    const service = TestBed.inject(Health);
    const http = TestBed.inject(HttpTestingController);

    service.load();

    const request = http.expectOne('/api/health');
    expect(request.request.method).toBe('GET');
    request.flush({ status: 'ok', service: 'api' });

    expect(service.status()).toBe('ok');
    http.verify();
  });
});
