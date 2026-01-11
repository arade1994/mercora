import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { catchError, throwError } from "rxjs";
import { ApiError } from "./api-error";

export const apiErrorInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((err: unknown) => {
            if (err instanceof HttpErrorResponse) {
              const apiError: ApiError = {
                message: extractProblemDetailsMessage(err) ?? 'Request failed.',
                status: err.status,
                raw: err.error,
              };
      
              // your middleware uses errorCode sometimes
              const code = err.error?.errorCode;
              if (typeof code === 'number') apiError.errorCode = code.toString();
      
              return throwError(() => apiError);
            }
      
            return throwError(() => ({ message: 'Unexpected error.' } as ApiError));
        })
    )
}

function extractProblemDetailsMessage(err: HttpErrorResponse): string | null {
    const body: any = err.error;
  
    // ProblemDetails { title, status, detail }
    if (typeof body?.detail === 'string') return body.detail;
  
    // ValidationProblemDetails { title, errors: { field: [msg] } }
    const errors = body?.errors;
    if (errors && typeof errors === 'object') {
      const firstKey = Object.keys(errors)[0];
      const first = errors[firstKey]?.[0];
      if (typeof first === 'string') return first;
    }
  
    if (typeof body?.title === 'string') return body.title;
  
    // fallback
    return typeof err.message === 'string' ? err.message : null;
  }