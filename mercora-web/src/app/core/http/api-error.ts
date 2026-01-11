export interface ApiError {
    message: string;
    status?: number;
    errorCode?: string;
    raw?: any;
}