
import { Metadata } from "next";
import "./globals.css";
import Navbar from "./nav/Navbar";
import ToasterProvider from "./providers/ToasterProvider";
import SignalRProvider from "./providers/SignalRProvider";

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
          <SignalRProvider>
            {children}
          </SignalRProvider>
        </main>
      </body>
    </html>
  );
}
