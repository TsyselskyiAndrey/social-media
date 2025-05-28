import React, { useEffect, useState } from "react";
import { loadStripe, Stripe } from "@stripe/stripe-js";
import Agent from "../../API/agent";
import "./SubscriptionsPage.css";

interface Plan {
  id: string;
  name: string;
  price: string;
  description: string;
  features: string[];
}

const SubscriptionsPage: React.FC = () => {
  const [plans, setPlans] = useState<Plan[]>([]);
  const [stripePromise, setStripePromise] = useState<Promise<Stripe | null> | null>(null);
  const [loading, setLoading] = useState<string | null>(null);
  const [isLoadingPlans, setIsLoadingPlans] = useState(true);

  useEffect(() => {
    const fetchPlansAndConfig = async () => {
      try {
        const [configResponse, plansResponse] = await Promise.all([Agent.Payment.getConfig(), Agent.Payment.getAllPlans()]);

        const publishableKey = configResponse.data;
        setStripePromise(loadStripe(publishableKey));

        setPlans(
          plansResponse.data.map((plan: any) => ({
            id: plan.priceId,
            name: plan.planName,
            price: plan.price,
            description: plan.description || "",
            features: plan.features || [],
          }))
        );
      } catch (error) {
        console.error("Failed to fetch config or plans:", error);
      } finally {
        setIsLoadingPlans(false);
      }
    };

    fetchPlansAndConfig();
  }, []);

  const handleCheckout = async (priceId: string) => {
    setLoading(priceId);
    try {
      const response = await Agent.Payment.createCheckoutSession({ priceId });
      const stripe = await stripePromise;

      if (!stripe) {
        throw new Error("Stripe failed to load");
      }

      await stripe.redirectToCheckout({ sessionId: response.sessionId });
    } catch (error) {
      console.error("Stripe checkout error:", error);
      alert("Something went wrong while starting checkout.");
    } finally {
      setLoading(null);
    }
  };

  return (
    <div className="subscriptions-container">
      <div className="subscriptions-content">
        <h1 className="title">Choose Your Plan</h1>

        {isLoadingPlans ? (
          <div className="loader">Loading plans...</div>
        ) : (
          <div className="plans-grid">
            {plans.map((plan) => (
              <div key={plan.id} className="plan-card">
                <h2 className="plan-name">{plan.name}</h2>
                <p className="plan-price">{plan.price}</p>
                <p className="plan-description">{plan.description}</p>
                <ul className="plan-features">
                  {plan.features.map((feature, index) => (
                    <li key={index}>✔ {feature}</li>
                  ))}
                </ul>
                <button className="choose-button" onClick={() => handleCheckout(plan.id)} disabled={loading === plan.id}>
                  {loading === plan.id ? "Redirecting..." : "Choose Plan"}
                </button>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default SubscriptionsPage;
