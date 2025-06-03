export interface Subscription {
  subscriptionId: number;
  name: string;
  description: string;
  productId: string;
  priceId: string;
  price: number;
  currency: string;
  interval: string;
  startDate: string;
  currentPeriodStart: string;
  currentPeriodEnd: string;
  status: string;
}
