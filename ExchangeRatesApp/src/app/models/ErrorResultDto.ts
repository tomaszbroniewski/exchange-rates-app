import { ExchangeRatesAppErrorCodes } from ".";


export interface ErrorResultDto {
    messages: string[];
    errorCode: ExchangeRatesAppErrorCodes;
    errorCodeName: string;
    errorLogId: string;
    techDetails?: TechDetails;
}

export interface TechDetails {
    methodName: string;
    exception: string;
    innerException?: string;
}