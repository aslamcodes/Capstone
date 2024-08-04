export function getErrorMessage(error: any, message?: string): string {
  return (
    error.response?.data?.message ??
    error.message ??
    message ??
    "Cannot Load Data"
  );
}
