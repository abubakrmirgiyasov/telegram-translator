import React, { FC, useState } from "react";
import Style from "./Sidebar.module.css";

const Sidebar: FC = () => {
  const [isMenuChecked, setIsMenuChecked] = useState<boolean>(false);

  const handleMenuClick = () => {
    setIsMenuChecked(!isMenuChecked);

    const sidebar: HTMLElement = document.getElementById("sidebar");
    sidebar.style.width = !isMenuChecked ? "5rem" : "18.5rem";
  };

  function getAt(index: number, elements: HTMLCollection): HTMLElement {
    return elements.item(index) as HTMLElement;
  }

  if (!isMenuChecked) {
    return (
      <React.Fragment>
        <div className={Style.sidebar} id={"sidebar"}>
          <h1>Telegram test</h1>
          <div>
            <form id={"search-form"} role={"search"}>
              <input
                id={"q"}
                type={"search"}
                aria-label={"Search"}
                placeholder={"Search..."}
                name={"q"}
              />
              <div id={"search-spinner"} aria-hidden={true} hidden={true} />
              <div className={"src-only"} aria-live={"polite"} />
            </form>
            <div
              id={"menuToggle"}
              className={Style.menuToggle}
              onClick={handleMenuClick}
            >
              <span
                style={{
                  transform: "rotate(0) translate(0 0)",
                }}
              ></span>
              <span style={{ opacity: "1" }}></span>
              <span style={{ transform: "rotate(0) translate(0 0)" }}></span>
            </div>
          </div>
          <nav>
            <ul>
              <li>
                <a href={`/contacts/1`}>Your Name</a>
              </li>
              <li>
                <a href={`/contacts/2`}>Your Friend</a>
              </li>
            </ul>
          </nav>
        </div>
        <div id={"detail"}></div>
      </React.Fragment>
    );
  } else {
    return (
      <React.Fragment>
        <div className={Style.sidebar} id={"sidebar"}>
          <div>
            <div
              id={"menuToggle"}
              className={Style.menuToggle}
              onClick={handleMenuClick}
            >
              <span
                style={{ transform: "rotate(45deg) translate(1px, -2px)" }}
              ></span>
              <span style={{ opacity: "0" }}></span>
              <span
                style={{ transform: "rotate(-45deg) translate(-4px, 0px)" }}
              ></span>
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
};

export default Sidebar;
