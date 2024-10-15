
import { Metadata } from "next";
import "./globals.css";
import Navbar from "./nav/Navbar";
import ToasterProvider from "./providers/ToasterProvider";
import SignalRProvider from "./providers/SignalRProvider";
import { getCurrentUser } from "./actions/authActions";

export const metadata: Metadata = {
  title: "Leilão",
  description: "App Leilão MicroServices",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const user = await getCurrentUser();
  const notifyUrl = process.env.NOTIFY_URL

  return (
    <html lang="en">
      <body>
        <ToasterProvider/>
        <Navbar/>
        <main className="container mx-auto p-10">
          <SignalRProvider user={user} notifyUrl={notifyUrl!}>
            {children}
          </SignalRProvider>
        </main>
      </body>
    </html>
  );
}
