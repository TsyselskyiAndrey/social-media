import React from 'react';

interface SuccessStepProps {
  onReset: () => void;
  
}

const SuccessStep: React.FC<SuccessStepProps> = ({ onReset }) => (
  <div>
    <h2>Пост успішно створено! 🎉</h2>
    <button onClick={onReset}>Створити ще один пост</button>
    <button onClick={() => window.location.href = '/feed'}>Повернутися до стрічки</button>
  </div>
);

export default SuccessStep;