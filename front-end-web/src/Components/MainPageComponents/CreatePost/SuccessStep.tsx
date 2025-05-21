import React from "react";
import { CheckCircle } from "lucide-react";

const SuccessStep: React.FC = () => {
  return (
    <div className="text-center p-8">
      <CheckCircle className="mx-auto mb-4 text-green-500" size={48} />
      <h2 className="text-2xl font-semibold mb-2">Допис опубліковано!</h2>
      <p className="text-gray-500">Ваш допис успішно збережено.</p>
    </div>
  );
};

export default SuccessStep;