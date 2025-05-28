import "./PaymentCompletionPages.css";
import { Link } from "react-router-dom";

function PaymentCancelPage() {
  return (
    <div className="payment-container cancel">
      <div className="payment-card">
        <div className="icon">❌</div>
        <h1>Payment Cancelled</h1>
        <p>We're sorry, the payment process was not completed.</p>
        <Link to="/" className="back-link">
          ← Back to Main
        </Link>
      </div>
    </div>
  );
}

export default PaymentCancelPage;
