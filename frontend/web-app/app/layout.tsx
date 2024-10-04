
import { Metadata } from "next";
import "./globals.css";
import Navbar from "./nav/Navbar";
import ToasterProvider from "./providers/ToasterProvider";

export const metadata: Metadata = {
  title: "Leilão",
  description: "App Leilão MicroServices",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <ToasterProvider/>
        <Navbar/>
        <main className="container mx-auto p-10">
          {children}
        </main>
      </body>
    </html>
  );
}
