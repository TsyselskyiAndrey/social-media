import React, { useState } from "react";
import Cards from "react-credit-cards-2";
import "react-credit-cards-2/dist/es/styles-compiled.css";
import "./CheckoutPage.css";
import googlePayMark from "../../Assets/google-pay-mark.png";

type FocusedField = "name" | "number" | "expiry" | "cvc" | undefined;

const CheckoutPage: React.FC = () => {
  const [cardInfo, setCardInfo] = useState<{
    number: string;
    name: string;
    expiry: string;
    cvc: string;
    focus: FocusedField;
  }>({
    number: "",
    name: "",
    expiry: "",
    cvc: "",
    focus: undefined,
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCardInfo({ ...cardInfo, [e.target.name]: e.target.value });
  };

  const handleInputFocus = (e: React.FocusEvent<HTMLInputElement>) => {
    setCardInfo({
      ...cardInfo,
      focus: ["name", "number", "expiry", "cvc"].includes(e.target.name) ? (e.target.name as FocusedField) : undefined,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert("Оплата отправлена!");
  };

  const handleGooglePay = () => {
    alert("Google Pay пока не реализован.");
  };

  return (
    <div className="payment-form-container">
      <h2 className="payment-title">Оплата картой</h2>
      <Cards number={cardInfo.number} name={cardInfo.name} expiry={cardInfo.expiry} cvc={cardInfo.cvc} focused={cardInfo.focus} />

      <form className="payment-form" onSubmit={handleSubmit}>
        <input
          type="tel"
          name="number"
          placeholder="Номер карты"
          value={cardInfo.number}
          onChange={handleInputChange}
          onFocus={handleInputFocus}
          inputMode="numeric"
          pattern="[0-9]{16}"
          maxLength={16}
          required
        />

        <input
          type="text"
          name="name"
          placeholder="Имя на карте"
          value={cardInfo.name}
          onChange={handleInputChange}
          onFocus={handleInputFocus}
          required
        />

        <input
          type="tel"
          name="expiry"
          placeholder="MM/YY"
          value={cardInfo.expiry}
          onChange={handleInputChange}
          onFocus={handleInputFocus}
          inputMode="numeric"
          pattern="(0[1-9]|1[0-2])\/\d{2}"
          maxLength={5}
          required
        />

        <input
          type="tel"
          name="cvc"
          placeholder="CVC"
          value={cardInfo.cvc}
          onChange={handleInputChange}
          onFocus={handleInputFocus}
          inputMode="numeric"
          pattern="[0-9]{3,4}"
          maxLength={4}
          required
        />
        <button type="submit">Pay</button>
      </form>
      <div className="divider-checkout ">
        <div className="line-checkout"></div>
        <p>&nbsp;&nbsp;OR&nbsp;&nbsp;</p>
        <div className="line-checkout"></div>
      </div>
      <button className="google-pay-button" onClick={handleGooglePay}>
        <img src={googlePayMark} alt="Google Pay" />
      </button>
    </div>
  );
};

export default CheckoutPage;
