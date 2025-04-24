import { Container } from "@mui/material";

export default function Footer() {
  return (
    <Container sx={{ marginTop: "auto", padding: "20px" }}>
      <footer>
        All rights reserved &copy; {new Date().getFullYear()} Social Media App
      </footer>
    </Container>
  );
}
