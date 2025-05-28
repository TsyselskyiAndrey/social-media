import "./PaymentCompletionPages.css";
import { Link } from "react-router-dom";

function PaymentSuccessPage() {
  return (
    <div className="payment-container success">
      <div className="payment-card">
        <div className="icon">✅</div>
        <h1>Payment Successful!</h1>
        <p>Thank you for your subscription. You now have access to all the amazing features!</p>
        <Link to="/" className="back-link">
          ← Back to Main
        </Link>
      </div>
    </div>
  );
}

export default PaymentSuccessPage;
