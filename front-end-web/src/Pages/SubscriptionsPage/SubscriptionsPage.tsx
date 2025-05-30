import React, { useEffect, useState } from "react";
import { loadStripe, Stripe } from "@stripe/stripe-js";
import Agent from "../../API/agent";
import "./SubscriptionsPage.css";
import useAuth from "../../Hooks/useAuth";

interface Plan {
  priceId: string;
  name: string;
  productId: string;
  price: string;
  description: string;
  features: string[];
}

const SubscriptionsPage: React.FC = () => {
  const [plans, setPlans] = useState<Plan[]>([]);
  const [stripePromise, setStripePromise] = useState<Promise<Stripe | null> | null>(null);
  const [loading, setLoading] = useState<string | null>(null);
  const [isLoadingPlans, setIsLoadingPlans] = useState(true);
  const { auth, setAuth } = useAuth();

  useEffect(() => {
    const fetchPlansAndConfig = async () => {
      try {
        const [configResponse, plansResponse] = await Promise.all([Agent.Payment.getConfig(), Agent.Payment.getAllPlans()]);

        const publishableKey = configResponse.data;
        setStripePromise(loadStripe(publishableKey));

        setPlans(
          plansResponse.data.map((plan: any) => ({
            priceId: plan.priceId,
            name: plan.planName,
            price: plan.price,
            productId: plan.productId,
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
    } finally {
      setLoading(null);
    }
  };

  const handleUpgradeSubscription = async (priceId: string) => {
    setLoading(priceId);
    try {
      const response = await Agent.Payment.upgradeSubscription({ priceId });
      const stripe = await stripePromise;

      if (!stripe) {
        throw new Error("Stripe failed to load");
      }

      await stripe.redirectToCheckout({ sessionId: response.sessionId });
    } catch (error) {
      console.error("Stripe checkout error:", error);
    } finally {
      setLoading(null);
    }
  };

  const handleCancelSubscription = async (priceId: string) => {
    setLoading(priceId);
    try {
      await Agent.Payment.cancelSubscription({ priceId });

      if (!auth) return;

      setAuth({
        ...auth,
        subscriptions: auth.subscriptions.filter((sub) => sub.priceId !== priceId),
      });
    } catch (error) {
      console.error("Failed to cancel subscription:", error);
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
            {plans.map((plan) => {
              const isElitePlan = plan.name.toLowerCase().includes("elite");
              const userSubscriptions = auth?.subscriptions || [];
              const userHasSubscription = userSubscriptions.length > 0;
              const currentSub = userSubscriptions.find((sub) => sub.productId === plan.productId);
              const userHasElite = userSubscriptions.some((sub) => sub.name?.toLowerCase().includes("elite"));
              let button;
              if (currentSub) {
                button = (
                  <button className="cancel-button" onClick={() => handleCancelSubscription(plan.priceId)} disabled={loading === plan.priceId}>
                    {loading === plan.priceId ? "Cancelling..." : "Cancel"}
                  </button>
                );
              } else if (userHasElite && !isElitePlan) {
                button = (
                  <button className="choose-button" disabled={true}>
                    Choose Plan
                  </button>
                );
              } else if (isElitePlan && !userHasElite && userHasSubscription) {
                button = (
                  <button className="choose-button" onClick={() => handleUpgradeSubscription(plan.priceId)} disabled={loading === plan.priceId}>
                    {loading === plan.priceId ? "Redirecting..." : "Upgrade"}
                  </button>
                );
              } else {
                button = (
                  <button className="choose-button" onClick={() => handleCheckout(plan.priceId)} disabled={loading === plan.priceId}>
                    {loading === plan.priceId ? "Redirecting..." : "Choose Plan"}
                  </button>
                );
              }
              return (
                <div key={plan.priceId} className="plan-card">
                  <h2 className="plan-name">{plan.name}</h2>
                  <p className="plan-price">{plan.price}</p>
                  <p className="plan-description">{plan.description}</p>
                  <ul className="plan-features">
                    {plan.features.map((feature, index) => (
                      <li key={index}>✔ {feature}</li>
                    ))}
                  </ul>
                  {button}
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default SubscriptionsPage;
