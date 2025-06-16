describe("Ultimate Verdict", () => {
  it("starts", () => {
    cy.visit("/");

    cy.get("body").contains("Ultimate Verdict");
  });
});
