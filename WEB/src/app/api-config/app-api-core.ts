export interface IApiConfig {
    readonly baseURL: string;
    readonly apiBaseUrl: string;
    readonly stripeKey: string;
    /**
     * Used to redirect to the Stripe admin dashboard
     */
    readonly stripeBaseUrl: string;
}
