import React from "react";

interface DownloadAppButtonsProps {
  apkLink: string;
  playStoreLink: string;
  appStoreLink: string;
}

const DownloadAppButtons: React.FC<DownloadAppButtonsProps> = ({
  apkLink,
  playStoreLink,
  appStoreLink,
}) => {
  const buttons = [
    {
      label: "Download on Play Store",
      link: playStoreLink,
      colorFrom: "from-green-50",
      colorTo: "to-green-100",
      hoverBorder: "hover:border-green-300",
    },
    {
      label: "Download on App Store",
      link: appStoreLink,
      colorFrom: "from-gray-50",
      colorTo: "to-gray-100",
      hoverBorder: "hover:border-gray-300",
    },
    {
      label: "Download APK",
      link: apkLink,
      colorFrom: "from-teal-50",
      colorTo: "to-teal-100",
      hoverBorder: "hover:border-teal-300",
    },
  ];

  return (
    <div className="space-y-4">
      {buttons.map((btn, idx) => (
        <a
          key={idx}
          href={btn.link}
          target="_blank"
          rel="noopener noreferrer"
          className={`flex items-center justify-center bg-white dark:bg-cyan-950 p-5 rounded-xl shadow-md hover:shadow-lg hover:bg-gradient-to-r ${btn.colorFrom} ${btn.colorTo} dark:hover:from-gray-700 dark:hover:to-gray-600 transition-all duration-300 ease-in-out cursor-pointer transform hover:scale-[1.02] hover:-translate-y-1 border border-gray-100 dark:border-cyan-900 ${btn.hoverBorder}`}
        >
          <span className="text-lg font-bold text-gray-900 dark:text-gray-100 transition-colors duration-300">
            {btn.label}
          </span>
        </a>
      ))}
    </div>
  );
};

export default DownloadAppButtons;
